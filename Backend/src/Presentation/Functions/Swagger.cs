#if ADD_SWAGGER
using System.Security.Claims;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Functions
{
    public static class SwaggerStartup
    {
        public static void AddSwaggerConfig(this IServiceCollection service)
        {
            service.AddSingleton<IOpenApiConfigurationOptions>(_ =>
            {
                var options = new OpenApiConfigurationOptions()
                {
                    Info = new OpenApiInfo()
                    {
                        Version = "1.0.0",
                        Title = "Langston-Wedding.com"
                    },
                    OpenApiVersion = OpenApiVersionType.V3,
                    IncludeRequestingHostName = true,
                    ForceHttps = false,
                    ForceHttp = false,
                };

                return options;
            });
        }

        public static async Task BuildSwagger(this IHost host)
        {
            var serviceProvider = host.Services;
            var context = serviceProvider.GetRequiredService<IOpenApiHttpTriggerContext>();
            await context.SetApplicationAssemblyAsync(AppDomain.CurrentDomain.BaseDirectory, false);
            var filter = new RouteConstraintFilter();
            var acceptor = new OpenApiSchemaAcceptor();
            var helper = new DocumentHelper(filter, acceptor);
            var document = new Document(helper);
            var json = await document
                .InitialiseDocument()
                .AddMetadata(context.OpenApiConfigurationOptions.Info)
                .AddServer(new SwaggerRequest(), "api", context.OpenApiConfigurationOptions)
                .AddNamingStrategy(context.NamingStrategy)
                .AddVisitors(context.GetVisitorCollection())
                .Build(context.ApplicationAssembly, context.OpenApiConfigurationOptions.OpenApiVersion)
                .ApplyDocumentFilters(context.GetDocumentFilterCollection())
                .RenderAsync(context.GetOpenApiSpecVersion(context.OpenApiConfigurationOptions.OpenApiVersion), context.GetOpenApiFormat("json"));

            var swaggerFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "swagger.json");
            await File.WriteAllTextAsync(swaggerFile, json);
            Console.WriteLine($"'{swaggerFile}' generated successfully.");
        }
    }

    // A mock request that's used to generate the swagger.json file
    public class SwaggerRequest : HttpRequestObject
    {
        public SwaggerRequest() : base(new SwaggerRequestData(new HttpRequestMessage(HttpMethod.Get, "http://localhost:7071/api/swagger.json")))
        {
        }

        public class SwaggerRequestData : HttpRequestData
        {
            private readonly HttpRequestMessage _httpRequest;

            public SwaggerRequestData(HttpRequestMessage httpRequest)
                : base(new SimpleFunctionContext())
            {
                _httpRequest = httpRequest ?? throw new ArgumentNullException(nameof(httpRequest));
            }

            public override Stream Body => Stream.Null;

            public override HttpHeadersCollection Headers => new HttpHeadersCollection(_httpRequest.Headers);

            public override IReadOnlyCollection<IHttpCookie> Cookies => new List<IHttpCookie>().AsReadOnly();

            public override Uri Url => _httpRequest.RequestUri!;

            public override IEnumerable<ClaimsIdentity> Identities => new List<ClaimsIdentity>();

            public override string Method => _httpRequest.Method.Method;

            public override HttpResponseData CreateResponse()
            {
                throw new NotImplementedException();
            }

            public class SimpleFunctionContext : FunctionContext
            {
                public SimpleFunctionContext()
                {
                    InvocationId = Guid.NewGuid().ToString();
                    FunctionId = Guid.NewGuid().ToString();
                    TraceContext = default!;
                    BindingContext = default!;
                    RetryContext = default!;
                    InstanceServices = null!;
                    FunctionDefinition = default!;
                    Items = new Dictionary<object, object>();
                    Features = default!;
                    CancellationToken = CancellationToken.None;
                }

                public override string InvocationId { get; }

                public override string FunctionId { get; }

                public override TraceContext TraceContext { get; }

                public override BindingContext BindingContext { get; }

                public override RetryContext RetryContext { get; }

                public override IServiceProvider InstanceServices { get; set; }

                public override FunctionDefinition FunctionDefinition { get; }

                public override IDictionary<object, object> Items { get; set; }

                public override IInvocationFeatures Features { get; }

                public override CancellationToken CancellationToken { get; }
            }
        }
    }
}
#endif