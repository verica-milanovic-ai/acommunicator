using System.Web;
using System.Web.Mvc;
using ACommunicator.Helpers;
using ACommunicator.Models;
using ACommunicator.Properties;

namespace ACommunicator.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            var endUserCookie = Request.Cookies.Get(CookieHelper.EndUserCookie)?.Value;
            var aUserCookie = Request.Cookies.Get(CookieHelper.AUserCookie)?.Value;

            if (!string.IsNullOrEmpty(endUserCookie) || !string.IsNullOrEmpty(aUserCookie))
            {
                return RedirectToAction("Index", "User");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                loginViewModel.Password = "";
                return View(loginViewModel);
            }

            var aUser = UserHelper.GetAUserByUsername(loginViewModel.Username);

            if (aUser != null && aUser.Password.Trim().Equals(loginViewModel.Password))
            {
                var userCookie = new HttpCookie(CookieHelper.AUserCookie, loginViewModel.Username);
                Response.Cookies.Add(userCookie);

                return RedirectToAction("Index", "User");
            }

            loginViewModel.Password = "";
            if (aUser == null)
            {
                ModelState.AddModelError("Username", Resources.NoUserWithSpecifiedUsername);
            }
            if (aUser != null && !aUser.Password.Equals(loginViewModel.Password))
            {
                ModelState.AddModelError("Password", Resources.WrongPassword);
            }
            return View(loginViewModel);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                registerViewModel.Password = "";
                registerViewModel.ConfirmPassword = "";
                return View(registerViewModel);
            }
            if (UserHelper.UsernameExists(registerViewModel.Username))
            {
                ModelState.AddModelError("Username", string.Format(Resources.UsernameAlreadyExists, registerViewModel.Username));
                registerViewModel.Password = "";
                registerViewModel.ConfirmPassword = "";
                return View(registerViewModel);
            }
            if (!registerViewModel.Password.Equals(registerViewModel.ConfirmPassword))
            {
                ModelState.AddModelError("ConfirmPassword", Resources.ConfirmPasswordMustMatchPassword);
                registerViewModel.Password = "";
                registerViewModel.ConfirmPassword = "";
                return View(registerViewModel);
            }


            var aUser = new AUser
            {
                Username = registerViewModel.Username,
                Password = registerViewModel.Password,
                Email = registerViewModel.Email
            };

            UserHelper.AddAUser(aUser);
            EmailHelper.SendWelcomeMail(aUser.Email);

            return RedirectToAction("AccountCreated", "User");
        }
    }
}