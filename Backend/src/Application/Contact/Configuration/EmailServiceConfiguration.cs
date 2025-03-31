using CrossCutting.Configuration;

namespace Application.Contact.Configuration;

[Configuration("Email")]
public class EmailServiceConfiguration
{
    public string[] DestinationEmail { get; set; } = [];
}