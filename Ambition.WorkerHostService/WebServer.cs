using System;
using Microsoft.Owin.Hosting;

namespace Ambition.WorkerHost
{
    public class WebServer
    {
        private IDisposable _webapp;

        public void Start()
        {
            _webapp = WebApp.Start<Startup>("http://localhost:8080");
        }

        public void Stop()
        {
            _webapp?.Dispose();
        }
    }
}