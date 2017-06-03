using System.Web;
using System.Web.Mvc;
using ACommunicator.Helpers;
using ACommunicator.Models;

namespace ACommunicator.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var aUserCookie = Request.Cookies.Get(CookieHelper.AUserCookie);
            var username = aUserCookie?.Value;

            // If aUserCookie is empty, go back to login screen.
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Home");
            }

            // TODO: fetch all end users for this admin user


            return View();
        }

        [HttpPost]
        public ActionResult Index(IndexViewModel indexViewModel)
        {
            var endUserCookie = new HttpCookie(CookieHelper.EndUserCookie, indexViewModel.SelectedEndUser.ToString());
            Response.Cookies.Add(endUserCookie);

            return RedirectToAction("EndUserIdex");
        }

        [HttpGet]
        public ActionResult EndUserIdex()
        {
            var endUserCookie = Request.Cookies.Get(CookieHelper.EndUserCookie);
            var endUserId = endUserCookie?.Value;

            if (string.IsNullOrEmpty(endUserId))
            {
                return RedirectToAction("NotFound","Error");
            }

            // TODO: fetch starting point options to display

            return View();
        }

        [HttpPost]
        public ActionResult Logout()
        {
            // TODO: remove both cookies - endUser and aUser
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogoutChild()
        {
            // TODO: remove endUser cookie
            return RedirectToAction("Index");
        }
    }
}