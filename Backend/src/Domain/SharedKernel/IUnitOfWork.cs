namespace Domain.SharedKernel;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    public delegate void SaveRequestedEvent( out Task task);
    protected static event SaveRequestedEvent? SaveRequestedRaised;

    // Method to request a save outside of DI
    public static async Task RequestSave()
    {
        Task? task = null;
        SaveRequestedRaised?.Invoke(out task);
        
        if (task != null) await task;
    }
}