using System.Threading;

namespace Ambition.WorkerHost.Interfaces
{
    public interface IExecutionItem
    {
        void Execute(CancellationToken cancellationToken);
    }
}