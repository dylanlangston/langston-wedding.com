using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Function;

var builder = FunctionsApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("./appsettings.json");

builder.ConfigureFunctionsWebApplication();

#if GENERATE_SWAGGER || DEBUG
builder.Services.AddSwaggerConfig();
#endif

var host = builder.Build();

#if GENERATE_SWAGGER || DEBUG
await host.BuildSwagger();
#endif

#if GENERATE_SWAGGER
Environment.Exit(0);
#endif

host.Run();

