﻿using SimpleBank.AcctManage.UI.Blazor.Server.v1.Contracts;
using SimpleBank.AcctManage.UI.Blazor.Server.v1.Data.Responses;
using System.Globalization;

namespace SimpleBank.AcctManage.UI.Blazor.Server.v1.Services.Base
{
    public class UserStorage : IUserStorage
    {
        private readonly ISbLocalStorage _sbLocalStorage;

        public UserStorage(ISbLocalStorage sbLocalStorage) =>
            _sbLocalStorage = sbLocalStorage ?? throw new ArgumentNullException(nameof(sbLocalStorage));


        public async Task SetUserInfo(LoginUserResponse loginUserResponse) =>
            await _sbLocalStorage.SetAsync(loginUserResponse);
        public async Task DeleteUserInfo() =>
            await _sbLocalStorage.DeleteAsync(new LoginUserResponse());
        public async Task<(bool, bool)> CheckAccessValidity()
        {
            var storageAccTokenDate = await _sbLocalStorage.GetAsync("AccessTokenExpiresAt");
            if (storageAccTokenDate == null) { return (false, false); }

            var format = new CultureInfo("pt-PT").DateTimeFormat;
            //var format = (IFormatProvider)CultureInfo.CurrentUICulture.DateTimeFormat;
            var style = DateTimeStyles.AssumeLocal;

            if (!DateTime.TryParse(storageAccTokenDate, format, style, out var accTokenDate)) { return (false, false); }
            return (true, accTokenDate > DateTime.UtcNow);
        }
        public async Task<(bool, bool)> CheckRefreshValidity()
        {
            var storageRfhTokenDate = await _sbLocalStorage.GetAsync("RefreshTokenExpiresAt");
            if (storageRfhTokenDate == null) { return (false, false); }

            var format = new CultureInfo("pt-PT").DateTimeFormat;
            var style = DateTimeStyles.AssumeLocal;

            if (!DateTime.TryParse(storageRfhTokenDate, format, style, out var rfhTokenDate)) { return (false, false); }
            return (true, rfhTokenDate > DateTime.UtcNow);
        }
        public async Task<string?> GetAccessToken() =>
            await _sbLocalStorage.GetAsync("AccessToken") ?? null;
        public async Task<string?> GetRefreshToken() =>
            await _sbLocalStorage.GetAsync("RefreshToken") ?? null;
        public async Task<string?> GetUserTokenId() =>
            await _sbLocalStorage.GetAsync("UserTokenId") ?? null;
        public async Task<string?> GetUserId() =>
            await _sbLocalStorage.GetAsync("UserId") ?? null;


    }
}
