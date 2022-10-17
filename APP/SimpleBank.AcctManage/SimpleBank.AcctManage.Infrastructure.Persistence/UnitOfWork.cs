﻿using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;
using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SimpleBankDbContext _context;

        public UnitOfWork(
            SimpleBankDbContext context,
            IUserRepository users,
            IAccountRepository accounts,
            IMovementRepository movements,
            ITransferRepository transfers,
            IUserTokenRepository userTokens,
            ISessionRepository sessions)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            Users = users ?? throw new ArgumentNullException(nameof(users));
            Accounts = accounts ?? throw new ArgumentNullException(nameof(accounts));
            Movements = movements ?? throw new ArgumentNullException(nameof(movements));
            Transfers = transfers ?? throw new ArgumentNullException(nameof(transfers));
            UserTokens = userTokens ?? throw new ArgumentNullException(nameof(userTokens));
            Sessions = sessions ?? throw new ArgumentNullException(nameof(sessions));
        }


        public IUserRepository Users { get; private set; }
        public IAccountRepository Accounts { get; private set; }
        public IMovementRepository Movements { get; private set; }
        public ITransferRepository Transfers { get; private set; }
        public IUserTokenRepository UserTokens { get; private set; }
        public ISessionRepository Sessions { get; private set; }


        public int SaveChanges() =>
            _context.SaveChanges();
        public async Task<int> SaveChangesAsync() =>
            await _context.SaveChangesAsync();
        public void Dispose() =>
            _context.Dispose();


    }
}
