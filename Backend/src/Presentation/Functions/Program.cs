using System.Reflection;
using CrossCutting.Extensions;
using Domain.Contact.Repositories;
using Functions.Middleware;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

#region Startup
// References to our assemblies for DI
Assembly[] assemblies = [
    Application.Assembly.Value,
    CrossCutting.Assembly.Value,
    Domain.Assembly.Value,
    Infrastructure.Assembly.Value
];

// Add appsettings.json file
builder.Configuration.AddJsonFile("./appsettings.json", false, true);

// Add Services
builder.AddServices(assemblies);

// Add Configurations
builder.AddConfigurations(assemblies);

// Add Commands/Queries
builder.AddCQRSDispatchers(assemblies);

// Add Domain Events
builder.AddDomainEventDispatcher(assemblies);

// Add EFCore + Repositories
builder.AddDbContext<ApplicationDbContext, UnitOfWork>(options =>
{
#if DEBUG
    options.UseInMemoryDatabase("DevelopmentDB");
#endif
}, assemblies);

#if DEBUG
builder.Services.AddMemoryCache();
#endif

// builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// builder.Services.AddScoped<IContactRequestRepository, ContactRequestRepository>();

// Configure Source Based JSON serializers
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Add(SourceGenerationContext.Default);
});

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
