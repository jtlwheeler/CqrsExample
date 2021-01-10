using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Banking.QueryProcessor.Domain.BankAccount;
using Banking.QueryProcessor.Queries.Handlers;
using Banking.QueryProcessor.Repository;
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

            IEnumerable<Transaction> transactions = new List<Transaction>
            {
                transaction1,
                transaction2,
                transaction3
            };

            var bankAccountRepository = new Mock<IRepository<Transaction>>();
            bankAccountRepository
                .Setup(mock => mock.Get(
                    It.IsAny<Expression<Func<Transaction, bool>>>(),
                    It.IsAny<Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>>>(),
                    It.IsAny<string>()))
                .Returns(Task.Run(() => transactions));

            var unitOfWork = new Mock<IRepositoryFacade>();
            unitOfWork
                .Setup(mock => mock.TransactionsRepository)
                .Returns(bankAccountRepository.Object);

            var handler = new TransactionQueryHandler(unitOfWork.Object);

            var actualTransactions = await handler.Handle(query, new CancellationToken());

            actualTransactions.Should().ContainInOrder(transaction1, transaction2, transaction3);
        }
    }
}
