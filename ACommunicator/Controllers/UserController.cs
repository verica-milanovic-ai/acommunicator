using ACommunicator.Models;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

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
            using (var db = new ACommunicatorEntities())
            {

                var usr = db.AUsers.SingleOrDefault(u => u.Username == loginViewModel.Username && u.Password == loginViewModel.Password);
                if(usr != null)
                {
                    Session["UserID"] = usr.Id.ToString();
                    Session["Username"] = usr.Username.ToString();
                    return RedirectToAction("Index", "Home");
                } else {
                    ModelState.AddModelError("", "Username or password is wrong.");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ACommunicatorEntities())
                {
                    var usr = new AUser()
                    {
                        Username = registerViewModel.Username,
                        Email = registerViewModel.Email,
                        Password = registerViewModel.Password
                    };
                    db.AUsers.Add(usr);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                            {
                                Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                            }
                        }
                    }
                    ModelState.Clear();
                    ViewBag.Message = usr.Username + " successfully registered.";
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }
    }
}