using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using System.Net;

namespace Function
{
    public class Contact
    {
        private readonly ILogger<Contact> _logger;

        public Contact(ILogger<Contact> logger)
        {
            _logger = logger;
        }

        [OpenApiOperation(operationId: "Contact", tags: new[] { "Contact" }, Summary = "Processes a contact request", Description = "Accepts a JSON request with contact details and validates the email.")]
        [OpenApiRequestBody( contentType: "application/json", typeof(ContactRequest))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "Successful response with greeting message")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "Bad request when JSON is invalid or email is incorrect")]
        [Function("Contact")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            if (req.HasJsonContentType())
            {
                try
                {
                    var contactRequest = await req.ReadFromJsonAsync<ContactRequest>();

                    if (contactRequest == null) throw new NullReferenceException("Failed to deserialize");

                    if (!contactRequest.IsValidEmail) return new BadRequestObjectResult("Invalid Email Address");

                    return new OkObjectResult($"Hello {contactRequest?.Name ?? "Unknown"}!");
                }
                catch (Exception err)
                {
                    _logger.LogError(err, "Failed to deserialize");
                }
            }
            return new BadRequestObjectResult("Invalid JSON");
        }
    }

    public partial class ContactRequest
    {
        [JsonRequired]
        public required string Name { get; set; }

        [JsonRequired]
        public required string Email { get; set; }

        [JsonRequired]
        public required string Message { get; set; }


        [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Singleline, 100)]
        private static partial Regex EmailRegex();

        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public bool IsValidEmail
        {
            get => EmailRegex().IsMatch(Email);
        }
    }
}
