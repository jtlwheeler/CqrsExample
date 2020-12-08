using CommandProcessor.Commands;
using CommandProcessor.Commands.Handlers;
using CommandProcessor.Events;
using CommandProcessor.Persistence;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(CommandProcessor.Startup))]
namespace CommandProcessor
{
    public class Startup: FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IEventBus, EventBusConsoleLogger>();
            builder.Services.AddSingleton<IGreetingRepository, InMemoryGreetingRepository>();
            builder.Services.AddSingleton<ICreateGreetingHandler, CreateGreetingHandler>();
            builder.Services.AddSingleton<ICommandBus, CommandBus>();
        }
    }
}
