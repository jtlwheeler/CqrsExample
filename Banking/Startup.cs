using Azure.Messaging.ServiceBus;
using Banking.CommandProcessor.Commands;
using Banking.CommandProcessor.Commands.Handlers;
using Banking.CommandProcessor.Entities;
using Banking.CommandProcessor.Events;
using Banking.CommandProcessor.Events.EventStore;
using Banking.QueryProcessor.Domain.BankAccount;
using Banking.QueryProcessor.Queries.Handlers;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(CommandProcessor.Startup))]
namespace CommandProcessor
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<Container>((s) =>
            {
                return ConfigureCosmosDb();
            });

            builder.Services.AddSingleton<ServiceBusClient>((s) =>
            {
                return ConfigureServiceBusClient();
            });

            builder.Services.AddDbContext<BankAccountContext>((s) =>
                s.UseCosmos(
                    Environment.GetEnvironmentVariable("CosmosDBAccountEndpoint"),
                    Environment.GetEnvironmentVariable("CosmosDBAccountKey"),
                    "BankDB"
                )
            );

            builder.Services.AddSingleton<IEventStore, CosmosDbEventStore>();
            builder.Services.AddSingleton<IEntityStore, EntityStore>();
            builder.Services.AddSingleton<IOpenBankAccountHandler, OpenBankAccountHandler>();
            builder.Services.AddSingleton<ICommandBus, CommandBus>();
            builder.Services.AddSingleton<IEventBus, ServiceBusEventBus>();
            builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            builder.Services.AddSingleton<IBankAccountQueryHandler, BankAccountQueryHandler>();
            builder.Services.AddSingleton<IMakeDepositHandler, MakeDepositHandler>();
        }

        private Container ConfigureCosmosDb()
        {
            var databaseId = "EventStore";
            var containerId = "Events";
            var connectionString = Environment.GetEnvironmentVariable("CosmosDBConnection");
            var client = new CosmosClient(connectionString);
            Database database = client.CreateDatabaseIfNotExistsAsync(databaseId).GetAwaiter().GetResult();
            return database.CreateContainerIfNotExistsAsync(containerId, "/AggregateRootId").GetAwaiter().GetResult();
        }

        private ServiceBusClient ConfigureServiceBusClient()
        {
            var connectionString = Environment.GetEnvironmentVariable("ServiceBusConnection");
            return new ServiceBusClient(connectionString);
        }

        //private BankAccountContext ConfigureBankAccountContext()
        //{
        //    var accountEndpoint = Environment.GetEnvironmentVariable("CosmosDBAccountEndpoint");
        //    var accountKey = Environment.GetEnvironmentVariable("CosmosDBAccountKey");
        //    var context = new BankAccountContext(accountEndpoint, accountKey);
        //    context.Database.EnsureCreatedAsync().GetAwaiter().GetResult();
        //    return context;
        //}
    }
}
