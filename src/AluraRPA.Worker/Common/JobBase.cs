namespace AluraRPA.Worker.Common;

[DisallowConcurrentExecution]
public abstract class JobBase : IJob
{
    protected ILogger<IJob> _logger { get; init; }

    protected JobBase(ILogger<IJob> logger) => _logger = logger;
    public abstract Task Execute(IJobExecutionContext context);
}