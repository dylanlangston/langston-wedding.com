using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Functions.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Application.Contact.Commands;

namespace Functions;

public class ContactFunction : BaseFunction
{
    public ContactFunction(
        ILogger<ContactFunction> logger,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
        ) : base(logger, commandDispatcher, queryDispatcher)
    {
    }

#if ADD_SWAGGER 
    [OpenApiOperation(operationId: "Contact", tags: new[] { "Contact" }, Summary = "Processes a contact request", Description = "Accepts a JSON request with contact details and validates the email.")]
    [OpenApiRequestBody( contentType: "application/json", typeof(ContactRequest))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(MessageResponse), Description = "Successful response with greeting message")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(MessageResponse), Description = "Bad request when JSON is invalid or email is incorrect")]
#endif
    [Function("Contact")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", "options")] HttpRequest req, CancellationToken cancellation)
    {
        _logger.LogInformation("Contact submission request.");

        if (req.HasJsonContentType())
        {
            string requestBody;
            try
            {
                requestBody = await new StreamReader(req.Body).ReadToEndAsync(cancellation);
                if (string.IsNullOrEmpty(requestBody))
                {
                    return new BadRequestObjectResult(new MessageResponse("Request body cannot be empty."));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading request body.");
                return new BadRequestObjectResult(new MessageResponse("Error reading request body."));
            }

            ContactRequest? contactRequest;
            try
            {
                contactRequest = JsonSerializer.Deserialize(requestBody, SourceGenerationContext.Default.ContactRequest);

                if (contactRequest == null)
                {
                    return new BadRequestObjectResult("Could not deserialize request body.");
                }

                var validationContext = new ValidationContext(contactRequest);
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(contactRequest, validationContext, validationResults, true))
                {
                    _logger.LogWarning("Contact request validation failed: {ValidationErrors}", validationResults);
                    return new BadRequestObjectResult(new MessageResponse(string.Join(' ', validationResults.Select(r => r.ErrorMessage))));
                }

            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Failed to deserialize contact request JSON.");
                return new BadRequestObjectResult(new MessageResponse($"Invalid JSON format: {jsonEx.Message}"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred processing the request.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            try
            {
                var command = new CreateContactRequestCommand(
                    contactRequest.Name,
                    contactRequest.Email,
                    contactRequest.Message);

                var result = await _commandDispatcher.Dispatch(command, cancellation);

                if (result.IsSuccess)
                {
                    _logger.LogInformation("Contact submitted successfully for email: {Email}", contactRequest.Email);
                    return new OkObjectResult(new MessageResponse("Contact information received successfully."));
                }
                else
                {
                    _logger.LogWarning("Contact submission failed: {Error}", result.Error);
                    return new BadRequestObjectResult(new MessageResponse(result.Error!));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred during contact submission processing.");
                return new ObjectResult(new MessageResponse("An unexpected error occurred processing your request."))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        return new BadRequestObjectResult("Request content type must be JSON.");
    }
}