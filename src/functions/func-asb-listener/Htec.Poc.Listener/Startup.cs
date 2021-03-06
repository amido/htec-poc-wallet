using Amido.Stacks.Messaging.Azure.ServiceBus.Serializers;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Core;
using Htec.Poc.Listener;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.DependencyInjection;
using Htec.Poc.Listener.Handlers;
using Microsoft.Extensions.Logging;
using Htec.Poc.Listener.Logging;
using Amido.Stacks.Data.Documents.CosmosDB;
using Amido.Stacks.Data.Documents.CosmosDB.Extensions;
using Htec.Poc.Domain;
using Htec.Poc.Application.Integration;
using Htec.Poc.Infrastructure.Repositories;
using Amido.Stacks.Configuration.Extensions;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Htec.Poc.Listener;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        RegisterDependentServices(builder);

        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }

    protected virtual void RegisterDependentServices(IFunctionsHostBuilder builder)
    {
        var configuration = LoadConfiguration(builder);

        // Register all handlers dynamically
        var definitions = typeof(UpdatePointsBalanceEventHandler).Assembly.GetImplementationsOf(typeof(IApplicationEventHandler<>));
        foreach (var definition in definitions)
        {
            builder.Services.AddTransient(definition.interfaceVariation, definition.implementation);
        }

        builder.Services
            .Configure<StacksListener>(configuration.GetSection(nameof(StacksListener)))
            .AddLogging(l => { l.AddSerilog(CreateLogger(configuration)); })
            .AddTransient(typeof(ILogger<>), typeof(LogAdapter<>));

        builder.Services.AddTransient<IMessageReader, CloudEventMessageSerializer>();

        builder.Services.AddSecrets();

        builder.Services.Configure<CosmosDbConfiguration>(configuration.GetSection("CosmosDb"));
        builder.Services.AddCosmosDB();
        builder.Services.AddTransient<IWalletRepository, CosmosDbWalletRepository>();

        var healthChecks = builder.Services.AddHealthChecks();
        healthChecks.AddCheck<CosmosDbDocumentStorage<Wallet>>("CosmosDB");
    }

    private static IConfiguration LoadConfiguration(IFunctionsHostBuilder builder)
    {
        return new ConfigurationBuilder()
            .SetBasePath(builder.GetContext().ApplicationRootPath)
            .AddJsonFile("appsettings.json", false)
            .AddEnvironmentVariables()
            .Build();
    }

    private static Logger CreateLogger(IConfiguration config)
    {
        return new LoggerConfiguration()
            .ReadFrom
            .Configuration(config)
            .CreateLogger();
    }
}
