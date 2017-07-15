using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using ACommunicator.Helpers;
using ACommunicator.Models;
using ACommunicator.Properties;

namespace ACommunicator.Controllers
{
    public class UserController : BaseController
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

            var endUserCookie = Request.Cookies.Get(CookieHelper.EndUserCookie);
            var endUserId = -1;
            int.TryParse(endUserCookie?.Value, out endUserId);

            // If endUserCookie is empty, go to index screen for aUser
            if (endUserId < 1)
            {
                return View(new IndexViewModel
                {
                    EndUserList = UserHelper.GetEndUserList(username),
                    SelectedEndUserId = endUserId
                });
            }

            // If endUserCookie isn't empty, go to index screen for endUser
            return RedirectToAction("EndUserIdex");
        }

        [HttpPost]
        public ActionResult Index(IndexViewModel indexViewModel)
        {
            CookieHelper.AddCookie(CookieHelper.EndUserCookie, indexViewModel.SelectedEndUserId.ToString(), Response);

            return RedirectToAction("EndUserIdex");
        }

        [HttpGet]
        public ActionResult RegisterChild()
        {
            return View(new RegisterChildViewModel());
        }

        [HttpPost]
        public ActionResult RegisterChild(RegisterChildViewModel registerChildViewModel)
        {

            if (!ModelState.IsValid)
            {
                return View(registerChildViewModel);
            }

            //Save picture on server
            UploadFile(registerChildViewModel);


            var aUsername = Request.Cookies.Get(CookieHelper.AUserCookie)?.Value;
            var aUser = UserHelper.GetAUserByUsername(aUsername);

            // Save created EndUser to DB
            var newEndUser = new EndUser()
            {
                Username = registerChildViewModel.Username,
                Name = string.IsNullOrEmpty(registerChildViewModel.Name)
                    ? registerChildViewModel.Username
                    : registerChildViewModel.Name,
                PicturePath = registerChildViewModel.Picture.FileName,
                AUsers = new List<AUser>()
            };

            newEndUser.AUsers.Add(aUser);
            newEndUser = UserHelper.AddEndUser(newEndUser);
            
            // Add endUser cookie for newly created End User
            CookieHelper.AddCookie(CookieHelper.EndUserCookie, newEndUser.Id.ToString(), Response);

            return RedirectToAction("EditEndUserProfile");
        }

        [HttpGet]
        public ActionResult EditEndUserProfile()
        {
            var endUserIdString = Request.Cookies.Get(CookieHelper.EndUserCookie)?.Value;
            if (string.IsNullOrEmpty(endUserIdString))
            {
                return RedirectToAction("SomethingWentWrong", "Error");
            }

            var endUserId = -1;
            int.TryParse(endUserIdString, out endUserId);
            var endUser = UserHelper.GetEndUserById(endUserId);

            if (endUser == null)
            {
                return RedirectToAction("SomethingWentWrong", "Error");
            }
            return View(new EditEndUserViewModel
            {
                EndUser = endUser,
                IsSuccessfullySaved = null
            });
        }

        [HttpPost]
        public ActionResult EditEndUserProfile(EditEndUserViewModel editEndUserViewModel)
        {
            if (!ModelState.IsValid) { return View(editEndUserViewModel); }

            editEndUserViewModel.IsSuccessfullySaved = UserHelper.UpdateEndUser(editEndUserViewModel.EndUser);
            return View(editEndUserViewModel);
        }

        [HttpGet]
        public ActionResult EndUserIdex()
        {
            var endUserCookie = Request.Cookies.Get(CookieHelper.EndUserCookie);
            var endUserId = endUserCookie?.Value;

            if (string.IsNullOrEmpty(endUserId))
            {
                return RedirectToAction("NotFound", "Error");
            }

            // TODO: fetch starting point options to display

            return View();
        }

        public ActionResult Logout()
        {
            CookieHelper.RemoveCookie(CookieHelper.EndUserCookie, Response);
            CookieHelper.RemoveCookie(CookieHelper.AUserCookie, Response);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogoutChild()
        {
            CookieHelper.RemoveCookie(CookieHelper.EndUserCookie, Response);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AccountCreated()
        {
            // TODO: display account created message
            return View();
        }

        private void UploadFile(RegisterChildViewModel registerChildViewModel)
        {

            try
            {
                var file = registerChildViewModel.Picture;
                var fileExtension = file.FileName.GetFileExtension();

                if (!string.IsNullOrEmpty(fileExtension) && file.ContentLength > 0)
                {
                    var aUsername = Request.Cookies.Get(CookieHelper.AUserCookie)?.Value;

                    // FileName pattern : a_<AdminUsername>_end_<EndUsername>_<DateTimeNow>.<fileExtension>
                    // Example: a_admin_end_childusername_20170622133700.jpeg
                    var fileName = string.Format(AppSettings.EndUserProfilePictureNamePattern,
                        aUsername,
                        registerChildViewModel.Username,
                        DateTime.Now.ToString("yyyyMMddHHmmss"),
                        fileExtension);

                    registerChildViewModel.Picture.SaveAs(Path.Combine(Server.MapPath(AppSettings.EndUserProfilePictureDirectory), fileName));
                }

            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
            }
        }
    }
}