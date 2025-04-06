using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Domain.SharedKernel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CrossCutting.Extensions;
public static class JSONExtensions
{
    public static void AddJsonOptions<T>(this IHostApplicationBuilder builder, params T[] jsonTypeInfos) where T : IJsonTypeInfoResolver
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            TypeInfoResolver = new DefaultJsonTypeInfoResolver()
        };

        options.TypeInfoResolverChain.Add(BaseSourceGenerationContext.Default);

        foreach (var jsonTypeInfo in jsonTypeInfos)
        {
            options.TypeInfoResolverChain.Add(jsonTypeInfo);
        }
        options.Converters.Add(new JsonStringEnumConverter());

        builder.Services.AddSingleton(options);
    }
}