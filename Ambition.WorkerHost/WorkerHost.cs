using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Ambition.WorkerHost.Interfaces;

namespace Ambition.WorkerHost
{
    public class WorkerHost
    {
        private static readonly Assembly[] Assemblies;
        private static CancellationTokenSource cancelTokenSource;
        private static CancellationToken token;

        private static readonly WorkerHost instance = new WorkerHost();

        public static WorkerHost Instance => instance;

        static WorkerHost()
        {
            Assemblies = GetWorkersAssemblies();
        }

        public void Start()
        {
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;

            foreach (var type in Assemblies.SelectMany(dll => dll.GetExportedTypes().Where(type => typeof(IWorker).IsAssignableFrom(type))))
            {
                Console.WriteLine($"Worker loaded: {type.Name}");
                var workerInstance = Activator.CreateInstance(type) as IWorker;
                MethodInfo startMethod = workerInstance.GetType().GetMethod("Start").MakeGenericMethod(new Type[] { type });

                startMethod.Invoke(workerInstance, new object[] {token});
                Task.Delay(1500);
            }
        }

        public void Cancel()
        {
            cancelTokenSource.Cancel();
        }

        private static Assembly[] GetWorkersAssemblies()
        {
            var workersPath = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            var assemblyMask = "Ambition.Worker.*.dll";
            var files = workersPath.GetFiles(assemblyMask);
            return files.Select(file => Assembly.LoadFile(file.FullName)).ToArray();
        }

        public string GetSummary()
        {
            return "WorkerHost summary";
        }
    }
}