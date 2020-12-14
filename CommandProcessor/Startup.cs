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
        private static readonly string EndpointUri = "[ENDPOINT_URI]";
        private static readonly string PrimaryKey = "[PRIMARY_KEY]";
        private string databaseId = "EventStore";
        private string containerId = "Events";

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
            var client = new CosmosClient(EndpointUri, PrimaryKey);
            Database database = client.CreateDatabaseIfNotExistsAsync(databaseId).GetAwaiter().GetResult();
            return database.CreateContainerIfNotExistsAsync(containerId, "/id").GetAwaiter().GetResult();
        }
    }
}
