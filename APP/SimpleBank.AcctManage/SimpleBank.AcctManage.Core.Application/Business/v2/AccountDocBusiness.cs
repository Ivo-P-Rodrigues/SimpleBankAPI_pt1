﻿using SimpleBank.AcctManage.Core.Application.Contracts.Business.v2;
using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;
using SimpleBank.AcctManage.Core.Domain;
using System.IO;
using static System.Net.WebRequestMethods;

namespace SimpleBank.AcctManage.Core.Application.Business.v2
{
    public class AccountDocBusiness : IAccountDocBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        public AccountDocBusiness(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public AccountDoc? Get(Guid docId) =>
            _unitOfWork.AccountDocs.GetWhere(doc => doc.Id == docId);

        public IEnumerable<AccountDoc> GetAll(Guid accountId) =>
            _unitOfWork.AccountDocs.GetAllWhere(m => m.AccountId == accountId);


        public async Task<bool> SaveAccountDocumentAsync(Guid accountId, string name, string contentType, Stream docStream) 
        {
            byte[] fileValue;
            using (Stream fileStream = docStream)
            {
                using (BinaryReader br = new BinaryReader(fileStream))
                {
                    fileValue = br.ReadBytes((Int32)fileStream.Length);
                }
            }

            AccountDoc accountDoc = new AccountDoc()
            {
                AccountId = accountId,
                Name = name + accountId.ToString(),
                Content = fileValue,
                DocType = contentType,
            };

            var savedDoc = await _unitOfWork.AccountDocs.DirectAddAsync(accountDoc);
            return savedDoc != null ? true : false;
        }





    }
}
