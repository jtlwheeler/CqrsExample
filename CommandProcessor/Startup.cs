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
    public class Startup: FunctionsStartup
    {
        private static readonly string EndpointUri = "[ENDPOINT_URI]";
        private static readonly string PrimaryKey = "[PRIMARY_KEY]";
        private string databaseId = "EventStore";
        private string containerId = "Events";

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<CosmosClient>((s) =>
            {
                return new CosmosClient(EndpointUri, PrimaryKey);
            });

            builder.Services.AddSingleton<IEventStore, CosmosDbEventStore>();
            builder.Services.AddSingleton<IEntityStore, EntityStore>();
            builder.Services.AddSingleton<IOpenBankAccountHandler, OpenBankAccountHandler>();
            builder.Services.AddSingleton<ICommandBus, CommandBus>();
        }
    }
}
