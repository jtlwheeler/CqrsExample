using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Banking.QueryProcessor.Domain.BankAccount;
using Banking.QueryProcessor.Queries.Handlers;
using Banking.QueryProcessor.Queries.Queries;
using FluentAssertions;
using Moq;
using Xunit;

namespace Banking.Tests.QueryProcessor.Queries.Handlers
{
    public class TransactionQueryHandlerTests
    {
        [Fact]
        public async void ShouldReturnTransactionsForBankAccount()
        {
            var query = new TransactionQuery("ABankAccountId");

            var bankAccount = new BankAccount();
            var transaction1 = new Transaction
            {
                Amount = 12.34m,
                Type = TransactionType.Deposit,
                Id = "1",
                Description = "Description 1"
            };

            var transaction2 = new Transaction
            {
                Amount = 99.99m,
                Type = TransactionType.Deposit,
                Id = "2",
                Description = "Description 2"
            };

            var transaction3 = new Transaction
            {
                Amount = 5663.99m,
                Type = TransactionType.Deposit,
                Id = "3",
                Description = "Description 3"
            };

            var transactions = new List<Transaction>
            {
                transaction1,
                transaction2,
                transaction3
            };
            
            var bankAccountRepository = new Mock<IBankAccountRepository>();
            bankAccountRepository
                .Setup(mock => mock.GetTransactions(query.BankAccountId))
                .Returns(Task.Run(() => transactions));

            var handler = new TransactionQueryHandler(bankAccountRepository.Object);

            var actualTransactions = await handler.Handle(query, new CancellationToken());

            actualTransactions.Should().ContainInOrder(transaction1, transaction2, transaction3);
        }
    }
}
