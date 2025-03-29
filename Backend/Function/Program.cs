using Function.Configuration;
using Function.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("./appsettings.json", false, true);

builder.AddConfigurationsFromAssembly(typeof(ConfigurationAttribute).Assembly);

#if ADD_SWAGGER
builder.Services.AddSwaggerConfig();
#endif

builder.ConfigureFunctionsWebApplication();

builder.UseMiddleware<ErrorHandlingMiddleware>();
builder.UseMiddleware<CorsMiddleware>();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

var host = builder.Build();

#if ADD_SWAGGER
await host.BuildSwagger();
#endif

#if GENERATE_SWAGGER
Environment.Exit(0);
#endif

host.Run();
