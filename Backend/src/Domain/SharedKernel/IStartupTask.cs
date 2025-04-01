namespace Domain.SharedKernel;

public delegate void StartupTaskRun<TSender, TArguments>(TSender sender, params IEnumerable<TArguments?> args);

public interface IStartupTask<TSender, TArguments>
{
    void Run(TSender sender, params IEnumerable<TArguments?> args);
}

public class StartupTask<TSender, TArguments> : IStartupTask<TSender, TArguments>
{
    private readonly StartupTaskRun<TSender, TArguments> _task;
    private StartupTask(StartupTaskRun<TSender, TArguments> task) => _task = task;
    public void Run(TSender sender, params IEnumerable<TArguments?> args) => _task(sender, args);
    public static StartupTask<TSender, TArguments> Create(StartupTaskRun<TSender, TArguments> r) => new StartupTask<TSender, TArguments>(r);

    public static implicit operator StartupTaskRun<TSender, TArguments>?(StartupTask<TSender, TArguments>? s) => s?._task ?? null;
    public static implicit operator StartupTask<TSender, TArguments>?(StartupTaskRun<TSender, TArguments>? r) => r != null ? new StartupTask<TSender, TArguments>(r) : null;
}