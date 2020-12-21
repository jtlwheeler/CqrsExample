using System.Threading.Tasks;

namespace CommandProcessor.Domain.BankAccount
{
    public interface IBankAccountRepository
    {
        public Task Save(BankAccount bankAccount);
        public Task<BankAccount> Get(string id);
    }
}
