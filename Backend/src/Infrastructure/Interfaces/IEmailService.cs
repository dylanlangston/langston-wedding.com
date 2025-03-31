
namespace Infrastructure.Interfaces;

public interface IEmailService
{
    Task SendContactNotificationAsync(string email, string name, string message, CancellationToken cancellationToken = default);
}
