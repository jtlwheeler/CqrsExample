using CommandProcessor.Commands;
using CommandProcessor.Commands.Entities;
using CommandProcessor.Commands.Handlers;
using CommandProcessor.Events.Persistence;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(CommandProcessor.Startup))]
namespace CommandProcessor
{
    public class Startup: FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IEventStore, InMemoryEventStore>();
            builder.Services.AddSingleton<IEntityStore, EntityStore>();
            builder.Services.AddSingleton<IOpenBankAccountHandler, OpenBankAccountHandler>();
            builder.Services.AddSingleton<ICommandBus, CommandBus>();
        }
    }
}
