﻿using SimpleBank.AcctManage.UI.Blazor.Server.Data;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Mapper;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Requests;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Responses;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Services
{
    public class AccountService
    {
        private readonly SimpleBankClient _client;
        private readonly EntityMapper _mapper;

        private readonly string _requestUri = "/api/v2/accounts/";

        public AccountService(
            SimpleBankClient client,
            EntityMapper mapper)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Account?> Get(Guid accountId)
        {
            var responseAccount = await _client.GetAsync<ResponseAccount>(_requestUri + accountId.ToString(), true);
            if (responseAccount == null) { return null; }
            return _mapper.Map(responseAccount);
        }

        public async Task<IEnumerable<Account>?> GetAll()
        {
            var responseAccount = await _client.GetAsync<IEnumerable<ResponseAccount>>(_requestUri + "getall", true);
            if (responseAccount == null) { return null; }
            return _mapper.Map(responseAccount);
        }

        public async Task<Account?> Create(AccountCreate accountCreate)
        {
            var requestAccountCreate = _mapper.Map(accountCreate);
            var responseAccount = await _client.PostAsync<ResponseAccount>(_requestUri, requestAccountCreate, true);
            if (responseAccount == null) { return null; }
            return _mapper.Map(responseAccount);
        }







    }
}


