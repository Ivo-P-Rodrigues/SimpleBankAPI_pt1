﻿using SimpleBank.AcctManage.UI.Blazor.Server.Contracts.Clients;
using SimpleBank.AcctManage.UI.Blazor.Server.Contracts.Mapper;
using SimpleBank.AcctManage.UI.Blazor.Server.Contracts.Services;
using SimpleBank.AcctManage.UI.Blazor.Server.Data;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Mapper;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Requests;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Responses;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Services
{
    public class MovementService : IMovementService
    {
        private readonly ISimpleBankClient _client;
        private readonly IEntityMapper _mapper;

        private readonly string _baseUri = "/api/v2/accounts/";


        public MovementService(
            ISimpleBankClient client,
            IEntityMapper mapper)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }



        public async Task<IEnumerable<Movement>?> GetAll(Guid accountId)
        {
            var rspMovements = await _client.GetAsync<IEnumerable<ResponseMovement>>(BuildUri(accountId, "/GetAll"), true);
            if (rspMovements == null) { return null; }
            return _mapper.Map(rspMovements);

        }



        //Innerworkings
        private string BuildUri(Guid accountId, string action = "", Guid? childId = null) =>
            string.Concat(
                _baseUri,
                accountId.ToString(),
                action,
                childId == null ? "" : childId.ToString());


    }
}

