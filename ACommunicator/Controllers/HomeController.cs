using System.Web.Mvc;
using ACommunicator.Models;

namespace ACommunicator.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}