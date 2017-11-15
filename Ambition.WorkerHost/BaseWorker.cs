using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Ambition.WorkerHost.Interfaces;

namespace Ambition.WorkerHost
{
    public abstract class BaseWorker : IWorker
    {
        public void Start<T>(CancellationToken cancellationToken)
        {
            Type currentType = typeof(T);
            string executingTypeFullName = $"{currentType.Namespace}.{currentType.Name}ExecutionItem";
            Type executingItemType = Assembly.Load(currentType.Namespace).GetType(executingTypeFullName);

            Dictionary<string, string> config = GetConfigSection(currentType.Name);
            int threadCount = config.ContainsKey("ThreadCount") && int.TryParse(config["ThreadCount"], out threadCount) ? threadCount : 1;

            for (int i = 0; i < threadCount; i++)
            {
                IExecutionItem sample = (IExecutionItem)Activator.CreateInstance(executingItemType);

                Task.Run(() => sample.Execute(cancellationToken), cancellationToken);

                //Thread thread = new Thread(sample.Execute);
                //thread.Start();
            }
        }

        public Dictionary<string, string> GetConfigSection(string sectionName)
        {
            return ConfigurationManager.GetSection(sectionName) is Hashtable hashTable
                ? hashTable.Cast<DictionaryEntry>().ToDictionary(d => d.Key.ToString(), d => d.Value.ToString())
                : new Dictionary<string, string>();
        }
    }
}