using System.Collections.Generic;
using System.Web.Http;

namespace Ambition.WorkerHost.Controllers
{
    [RoutePrefix("Manage")]
    public class ManageController : ApiController
    {
        public string Get()
        {
            return WorkerHost.Instance.GetSummary();
        }

        [Route("Start")]
        [HttpPost]
        public void Start()
        {
            WorkerHost.Instance.Start();
        }

        [Route("Manage/Stop")]
        [HttpPost]
        public void Stop()
        {
            WorkerHost.Instance.Cancel();
        }
    }
}