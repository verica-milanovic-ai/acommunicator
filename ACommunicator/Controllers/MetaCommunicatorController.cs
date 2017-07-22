using System.Web.Mvc;
using ACommunicator.Helpers;
using ACommunicator.Models;

namespace ACommunicator.Controllers
{
    public class MetaCommunicatorController : Controller
    {
        // GET: MetaCommunicator
        public ActionResult Index()
        {
            var aUserCookie = Request.Cookies.Get(CookieHelper.AUserCookie);
            var username = aUserCookie?.Value;

            // If aUserCookie is empty, go back to login screen.
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Home");
            }

            var endUserCookie = Request.Cookies.Get(CookieHelper.EndUserCookie);
            var endUserId = -1;
            int.TryParse(endUserCookie?.Value, out endUserId);

            // If endUserCookie is empty, go to index screen for aUser
            return View(new IndexViewModel
            {
                EndUserList = UserHelper.GetEndUserList(username),
                SelectedEndUserId = endUserId
            });
        }
    }
}