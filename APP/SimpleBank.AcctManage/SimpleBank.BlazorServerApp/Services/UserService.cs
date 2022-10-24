﻿using Microsoft.AspNetCore.Components;
using SimpleBank.BlazorServerApp.Contracts;
using SimpleBank.BlazorServerApp.Data.Requests;
using SimpleBank.BlazorServerApp.Data.Responses;
using SimpleBank.BlazorServerApp.Services.Base;

namespace SimpleBank.BlazorServerApp.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly string _requestUri;

        public UserService(HttpClient httpClient, IUserStorage userStorage, IConfiguration configuration) : base(httpClient, userStorage, configuration)
        {
            _requestUri = Configuration["SbApiEndPointsAddresses:Users"];
        }

        public async Task<CreateUserResponse?> Create(CreateUserRequest createUserRequest)
        {
            var httpRsp = await PostAsync(_requestUri, createUserRequest);
            if (httpRsp == null) { return null; }

            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadFromJsonAsync(typeof(CreateUserResponse)) as CreateUserResponse;
            }
            return null;
        }

        public async Task<bool> Login(LoginUserRequest loginUserRequest) =>
            await RegisterLogin(loginUserRequest);
        public async Task<bool> Logout() =>
            await RegisterLogout();

        private ComponentEndpointConventionBuilder componentEndpointConventionBuilder;
        private void bla()
        {
            //NavigationManager
            //ComponentBase bla = new ComponentBase();
        }

    }
}




//httpRsp.RequestMessage
//httpRsp.Content.Headers

//var custome‌​rJsonString = await httpRsp.Content.ReadAsStringAsync()
//JsonConvert.DeserializeObject<Customer>(custome‌​rJsonString)

//_httpClient.PostAsync()
//_httpClient.GetAsync()