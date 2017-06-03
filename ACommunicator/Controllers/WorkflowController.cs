using System.Web.Mvc;

namespace ACommunicator.Controllers
{
    public class WorkflowController : Controller
    {
        [HttpGet]
        public ActionResult Option()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Option(object viewModel)
        {
            return View();
        }
    }
}