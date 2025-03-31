namespace Domain.SharedKernel;

public interface IDispatcher<in T>
{
    Task<Result> Dispatch<TDispatched>(TDispatched dispatched, CancellationToken cancellation = default) where TDispatched : T;
}