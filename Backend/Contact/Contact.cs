using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Contact
{
    public class Contact
    {
        private readonly ILogger<Contact> _logger;

        private string fromAddress;
        private string toAddress;

        public Contact(ILogger<Contact> logger)
        {
            fromAddress = Environment.GetEnvironmentVariable("FromEmail")!;
            toAddress = Environment.GetEnvironmentVariable("ToEmail")!;

            _logger = logger;
        }

        [Function("Contact")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            if (req.HasJsonContentType()) {
                try {
                    var contactRequest = await req.ReadFromJsonAsync<ContactRequest>();
                    return new OkObjectResult($"Hello {contactRequest?.FirstName ?? "Unknown"}!");
                } catch (Exception err) {
                    _logger.LogError(err, "Failed to deserialize");
                }
            }
            return new BadRequestObjectResult("Invalid JSON");
        }
    }

    public class ContactRequest
    {
        [JsonRequired]
        public required string FirstName { get; set; }

        [JsonRequired]
        public required string LastName { get; set; }

        [JsonRequired]
        public required string Email { get; set; }

        [JsonRequired]
        public required string Message { get; set; }

        public string? Phone { get; set; }

        public bool IsValidEmail()
        {
#if DEBUG
            if (Email == "test") return true;
#endif

            var regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$");
            return regex.IsMatch(Email!);
        }
    }
}