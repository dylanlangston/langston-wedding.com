using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Function.Configuration;

namespace Function.Middleware;

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
        var httpRequest = await context.GetHttpRequestDataAsync();
        if (httpRequest != null && httpRequest.Method == HttpMethod.Options.Method)
        {
            _logger.LogInformation($"Requested Options for `{httpRequest.Url.AbsolutePath}`");

            var response = httpRequest.CreateResponse(HttpStatusCode.InternalServerError);

            response.StatusCode = HttpStatusCode.NoContent;

            response.Headers.Add("Access-Control-Allow-Origin", string.Join(',', _config.AllowedOrigins));
            response.Headers.Add("Access-Control-Allow-Methods", string.Join(',', _config.AllowedMethods));
            response.Headers.Add("Access-Control-Allow-Headers", string.Join(',', _config.AllowedHeaders));

            context.GetInvocationResult().Value = response;
            return;
        }

        await next(context);
    }
}
