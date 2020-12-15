using CommandProcessor.Commands;
using CommandProcessor.Commands.Entities;
using CommandProcessor.Commands.Handlers;
using CommandProcessor.Events.Persistence;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
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

            builder.Services.AddSingleton<IEventStore, CosmosDbEventStore>();
            builder.Services.AddSingleton<IEntityStore, EntityStore>();
            builder.Services.AddSingleton<IOpenBankAccountHandler, OpenBankAccountHandler>();
            builder.Services.AddSingleton<ICommandBus, CommandBus>();
        }

        private Container ConfigureCosmosDb()
        {
            var databaseId = "EventStore";
            var containerId = "Events";
            var connectionString = Environment.GetEnvironmentVariable("CosmosDBConnection");
            var client = new CosmosClient(connectionString);
            Database database = client.CreateDatabaseIfNotExistsAsync(databaseId).GetAwaiter().GetResult();
            return database.CreateContainerIfNotExistsAsync(containerId, "/EntityId").GetAwaiter().GetResult();
        }
    }
}
