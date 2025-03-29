using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Function.Middleware;

public class ErrorHandlingMiddleware : IFunctionsWorkerMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var httpRequest = await context.GetHttpRequestDataAsync();
            if (httpRequest != null)
            {
                _logger.LogError(ex, "Unhandled exception in function {FunctionName}", context.FunctionDefinition.Name);

                var response = httpRequest.CreateResponse(HttpStatusCode.InternalServerError);
                response.Headers.Add("Content-Type", "application/json");
                await response.WriteStringAsync("{\"error\":\"An unexpected error occurred. Please try again later.\"}");
                context.GetInvocationResult().Value = response;
            }
            else throw;
        }
    }
}
