namespace Functions.DTOs;

public class MessageResponse
{
    public MessageResponse(string message) {
        Message = message;
    }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}