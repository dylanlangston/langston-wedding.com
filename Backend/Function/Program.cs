using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("./appsettings.json", false, true);

#if ADD_SWAGGER
builder.Services.AddSwaggerConfig();
#endif

builder.ConfigureFunctionsWebApplication();

builder.UseMiddleware<ErrorHandlingMiddleware>();

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
