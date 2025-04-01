using System.Reflection;
using CrossCutting.Extensions;
using Domain.Contact.Aggregates;
using Functions.Middleware;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = FunctionsApplication.CreateBuilder(args);

#region Startup
// References to our assemblies for DI
Assembly[] assemblies = [
    Assembly.GetExecutingAssembly(),
    Application.Assembly.Value,
    CrossCutting.Assembly.Value,
    Domain.Assembly.Value,
    Infrastructure.Assembly.Value
];

// Add Configurations
builder.AddConfigurations(assemblies);

// Add appsettings.json file
builder.Configuration.AddJsonFile("./appsettings.json", false, true);

// Add Services
builder.AddServices(assemblies);

// Add Commands/Queries
builder.AddCQRSDispatchers(assemblies);

// Add Domain Events
builder.AddDomainEventDispatcher(assemblies);

// Add EFCore + Repositories
builder.AddDbContext<ApplicationDbContext, UnitOfWork>(options =>
{
#if DEBUG
    options.UseInMemoryDatabase("DevelopmentDB");
#else
    options.UseCosmos(Environment.GetEnvironmentVariable("CosmosDBConnectionString")!, "Database");
#endif
}, assemblies);

// Add Repos with config
builder.AddRepositories((Type type, EntityTypeBuilder builder) =>
{
#if !DEBUG
    builder.ToContainer(type.Name);
    builder.HasPartitionKey(nameof(BaseEntity<Guid>.Id));
#endif
}, assemblies);

#if DEBUG
builder.Services.AddMemoryCache();
#endif

// Configure Source Based JSON serializers
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Add(SourceGenerationContext.Default);
});

// Support Compression
builder.Services.AddRequestDecompression();
builder.Services.AddResponseCompression();

// Support CORs

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

host.Run();
