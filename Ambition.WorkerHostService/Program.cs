using Topshelf;

namespace Ambition.WorkerHost
{
    class Program
    {
        static void Main(string[] args)
        {
            WorkerHost.Instance.Start();

            HostFactory.Run(x =>
            {
                x.Service<WebServer>(s =>
                {
                    s.ConstructUsing(name => new WebServer());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("This is a demo of a Windows Service using Topshelf.");
                x.SetDisplayName("Self Host Web API Demo");
                x.SetServiceName("AspNetSelfHostDemo");
            });
        }
    }
}
