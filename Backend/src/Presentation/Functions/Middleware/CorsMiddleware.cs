using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Functions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.WebApiCompatShim;

namespace Functions.Middleware;

public class CorsMiddleware : IFunctionsWorkerMiddleware
{
    private readonly CorsConfiguration _config;
    private readonly ILogger<CorsMiddleware> _logger;

    public CorsMiddleware(CorsConfiguration config, ILogger<CorsMiddleware> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var httpContext = context.GetHttpContext();
        httpContext?.Response.OnStarting(() =>
        {
            httpContext?.Response?.Headers.AppendList("Access-Control-Allow-Origin", _config.AllowedOrigins);
            httpContext?.Response?.Headers.AppendList("Access-Control-Allow-Methods", _config.AllowedMethods);
            httpContext?.Response?.Headers.AppendList("Access-Control-Allow-Headers", _config.AllowedHeaders);
            return Task.CompletedTask;
        });

        var httpRequest = httpContext.GetHttpRequestMessage();
        if (httpRequest != null && httpRequest.Method == HttpMethod.Options)
        {
            _logger.LogInformation($"Requested Options for `{httpRequest.RequestUri?.AbsolutePath}`");

            var response = httpRequest.CreateResponse(HttpStatusCode.InternalServerError);

            response.StatusCode = HttpStatusCode.NoContent;

            context.GetInvocationResult().Value = response;
            return;
        }

        await next(context);

        return;
    }
}
