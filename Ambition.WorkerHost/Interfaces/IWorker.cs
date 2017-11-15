using System.Threading;

namespace Ambition.WorkerHost.Interfaces
{
    public interface IWorker
    {
        void Start<T>(CancellationToken cancellationToken);
    }
}