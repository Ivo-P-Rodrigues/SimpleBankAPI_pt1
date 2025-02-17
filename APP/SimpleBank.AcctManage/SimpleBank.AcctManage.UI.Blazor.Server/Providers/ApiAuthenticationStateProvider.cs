﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Providers
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedLocalStorage _localStorage;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly IConfiguration _configuration;

        private readonly string _tokenIdKey;
        private readonly string _accessTokenKey;
        private readonly string _refreshTokenKey;

        public ApiAuthenticationStateProvider(
            ProtectedLocalStorage localStorage,
            JwtSecurityTokenHandler tokenHandler,
            IConfiguration configuration)
        {
            _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
            _tokenHandler = tokenHandler ?? throw new ArgumentNullException(nameof(tokenHandler));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _tokenIdKey = _configuration["StorageCustomKeys:tokenId"] ?? throw new ArgumentNullException("Invalid storage key: tokenId");
            _accessTokenKey = _configuration["StorageCustomKeys:accessToken"] ?? throw new ArgumentNullException("Invalid storage key: accessToken");
            _refreshTokenKey = _configuration["StorageCustomKeys:refreshToken"] ?? throw new ArgumentNullException("Invalid storage key: refreshToken");
        }


        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var savedToken = await _localStorage.GetAsync<string>(_accessTokenKey);

                if (string.IsNullOrWhiteSpace(savedToken.Value))
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var tokenContent = _tokenHandler.ReadJwtToken(savedToken.Value);
                if (tokenContent.ValidTo < DateTime.Now)
                {
                    await _localStorage.DeleteAsync(_tokenIdKey);
                    await _localStorage.DeleteAsync(_accessTokenKey);
                    await _localStorage.DeleteAsync(_refreshTokenKey);
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var claims = ParseClaims(tokenContent);
                var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
                return new AuthenticationState(user);
            }
            catch (Exception)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public async Task LoggedIn()
        {
            var savedToken = await _localStorage.GetAsync<string>(_accessTokenKey);
            var tokenContent = _tokenHandler.ReadJwtToken(savedToken.Value);

            var claims = ParseClaims(tokenContent);
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
        }

        public void LoggedOut()
        {
            var nobody = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(nobody));
            NotifyAuthenticationStateChanged(authState);
        }

        private IList<Claim> ParseClaims(JwtSecurityToken tokenContent)
        {
            var claims = tokenContent.Claims.ToList();
            claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));
            return claims;
        }





    }
}
