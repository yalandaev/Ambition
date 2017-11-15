using System.Threading;

namespace Ambition.WorkerHost.Interfaces
{
    public abstract class ExecutionItem : IExecutionItem
    {
        public abstract void Execute(CancellationToken cancellationToken);
    }
}