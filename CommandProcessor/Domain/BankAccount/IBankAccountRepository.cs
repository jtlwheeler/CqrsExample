using System.Threading.Tasks;

namespace CommandProcessor.Domain.BankAccount
{
    public interface IBankAccountRepository
    {
        public Task Save(BankAccount bankAccount);
    }
}
