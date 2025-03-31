namespace Domain.SharedKernel;

public interface IHandler<in IHandle>
{
    Task<Result> Handle(IHandle handle, CancellationToken cancellation = default);
}