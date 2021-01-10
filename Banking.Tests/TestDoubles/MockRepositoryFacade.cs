using Banking.QueryProcessor.Domain.BankAccount;
using Banking.QueryProcessor.Repository;
using Moq;

namespace Banking.Tests.TestDoubles
{
    public class MockBankingRepository
    {
        public Mock<IRepositoryFacade> MockRepositoryFacade { get; private set; }
        public Mock<IRepository<Transaction>> MockTransactionRepository { get; private set; }
        public Mock<IRepository<BankAccount>> MockBankAccountRepository { get; private set; }

        public MockBankingRepository()
        {
            MockRepositoryFacade = new Mock<IRepositoryFacade>();

            MockTransactionRepository = new Mock<IRepository<Transaction>>();
            MockRepositoryFacade
                .Setup(mock => mock.TransactionsRepository)
                .Returns(MockTransactionRepository.Object);

            MockBankAccountRepository = new Mock<IRepository<BankAccount>>();
            MockRepositoryFacade
                .Setup(mock => mock.BankAccountRepository)
                .Returns(MockBankAccountRepository.Object);
        }
    }
}