using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("./appsettings.json", false, true);

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

host.Run();

