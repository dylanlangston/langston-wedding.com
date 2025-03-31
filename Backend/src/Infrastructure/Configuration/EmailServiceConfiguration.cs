using CrossCutting.Configuration;

namespace Infrastructure.Configuration;

[Configuration("Email")]
public class EmailServiceConfiguration
{
    public string? DestinationEmail { get; set; }
}