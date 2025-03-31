using CrossCutting.Configuration;

namespace Functions.Configuration;

[Configuration("CORS")]
public class CorsConfiguration
{
    public string[] AllowedOrigins { get; set; } = [];
    public string[] AllowedMethods { get; set; } = [];
    public string[] AllowedHeaders { get; set; } = [];
}