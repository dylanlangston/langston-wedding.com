using Functions.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.DurableTask.Client;
using Microsoft.DurableTask.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Presentation.Core;


var builder = FunctionsApplication.CreateBuilder(args);

#region Startup
builder.Services.AddDurableTaskClient(b => {
    b.Configure(c => {
        // c.DataConverter = new PolymorphicJsonDataConverter();
    });
    b.RegisterDirectly();
    b.UseGrpc();
});
// Configure HostBuilder for our app
builder.ConfigureHostBuilder<DurableFunctionsDomainEventQueue, DurableFunctionsDomainEventWorker>(new () {
    ConfigureLogging = false,
    JsonTypeInfos = [SourceGenerationContext.Default]
});

// Add Source Based Json Serializers to HTTP Context
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Add(SourceGenerationContext.Default);
});

// Support Compression
builder.Services.AddRequestDecompression();
builder.Services.AddResponseCompression();

// Azure Function Web App
builder.ConfigureFunctionsWebApplication();

// Generic Error Handling
builder.UseMiddleware<ErrorHandlingMiddleware>();

// Application Insights
builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();
#endregion

#region Swagger
#if ADD_SWAGGER
builder.Services.AddSwaggerConfig();
#endif

var host = builder.Build();

#if ADD_SWAGGER
await host.BuildSwagger();
#endif

#if GENERATE_SWAGGER
Environment.Exit(0);
#endif
#endregion

CancellationTokenSource cts = new();

await host.RunAsync(cts.Token);