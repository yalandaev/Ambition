using System;
using System.Threading;
using Ambition.WorkerHost.Interfaces;

namespace Ambition.Worker.Quartz
{
    public class QuartzWorkerExecutionItem : ExecutionItem, IExecutionItem
    {
        public override void Execute(CancellationToken cancellationToken)
        {
            int number = GetRandomNumber();
            while (true)
            {
                try
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        Console.WriteLine("Worker {0} cancelling work", number);
                        return;
                    }
                        
                    DoWork(number);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Thread {0}: ERROR", number);
                }
            }
        }

        private void DoWork(int num)
        {
            var rnd = GetRandomNumber();
            Console.WriteLine("Worker {0} is working. Number:{1}", num, rnd);
            Thread.Sleep(3000);
        }

        private static int GetRandomNumber()
        {
            Random rnd = new Random();
            return rnd.Next(65536);
        }
    }
}