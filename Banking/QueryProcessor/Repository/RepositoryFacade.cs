using System;
using System.Threading.Tasks;
using Banking.QueryProcessor.Domain.BankAccount;

namespace Banking.QueryProcessor.Repository
{
    public class RepositoryFacade : IRepositoryFacade
    {
        private bool disposed = false;
        private readonly BankContext context;
        public IRepository<Transaction> TransactionsRepository { get; private set; }
        public IRepository<BankAccount> BankAccountRepository { get; private set; }

        public RepositoryFacade(BankContext context)
        {
            this.context = context;
            context.Database.EnsureCreated();

            TransactionsRepository = new Repository<Transaction>(context);
            BankAccountRepository = new Repository<BankAccount>(context);
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
