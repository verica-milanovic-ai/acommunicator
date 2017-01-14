using ACommunicator.Models;
using System.Web.Mvc;

namespace ACommunicator.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            return View("Index");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel registerViewModel)
        {
            return View("Index");
        }
    }
}