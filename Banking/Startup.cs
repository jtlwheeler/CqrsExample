using Azure.Messaging.ServiceBus;
using Banking.CommandProcessor.Commands;
using Banking.CommandProcessor.Commands.Handlers;
using Banking.CommandProcessor.Entities;
using Banking.CommandProcessor.Events;
using Banking.CommandProcessor.Events.EventStore;
using Banking.QueryProcessor.Domain.BankAccount;
using Banking.QueryProcessor.Repository;
using MediatR;
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
            builder.Services.AddMediatR(typeof(Startup));
            builder.Services.AddSingleton((s) =>
            {
                return ConfigureCosmosDb();
            });

            builder.Services.AddSingleton((s) =>
            {
                return ConfigureServiceBusClient();
            });


            builder.Services.AddDbContext<BankAccountContext>((options) =>
                SqlServerDbContextOptionsExtensions.UseSqlServer(
                    options,
                    Environment.GetEnvironmentVariable("SQLServerDbConnectionString")
                )
            );

            builder.Services.AddSingleton<IEventStore, CosmosDbEventStore>();
            builder.Services.AddSingleton<IEntityStore, EntityStore>();
            builder.Services.AddSingleton<IOpenBankAccountHandler, OpenBankAccountHandler>();
            builder.Services.AddSingleton<ICommandBus, CommandBus>();
            builder.Services.AddSingleton<IEventBus, ServiceBusEventBus>();
            builder.Services.AddScoped<IRepositoryFacade, RepositoryFacade>();
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
    }
}
