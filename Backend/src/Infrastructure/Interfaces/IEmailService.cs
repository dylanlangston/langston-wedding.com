
namespace Infrastructure.Interfaces;

public interface IEmailService
{
    Task SendContactNotificationAsync(Guid contactRequestId, string email, string name, string message, CancellationToken cancellationToken = default);
}
