using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using ACommunicator.Helpers;
using ACommunicator.Helpers.Google;
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
            return RedirectToAction("EndUserIndex");
        }

        [HttpPost]
        public ActionResult Index(IndexViewModel indexViewModel)
        {
            CookieHelper.AddCookie(CookieHelper.EndUserCookie, indexViewModel.SelectedEndUserId.ToString(), Response);

            return RedirectToAction("EndUserIndex");
        }

        [HttpGet]
        public ActionResult EditAUserProfile()
        {
            var aUserCookie = Request.Cookies.Get(CookieHelper.AUserCookie);
            var username = aUserCookie?.Value;

            // If aUserCookie is empty, go back to login screen.
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Home");
            }

            var aUser = UserHelper.GetAUserByUsername(username);
            if (aUser != null)
            {
                aUser.Password = aUser.Password?.Trim() ?? string.Empty;
                aUser.Name = aUser.Name?.Trim() ?? string.Empty;
                aUser.Username = aUser.Username?.Trim() ?? string.Empty;
                aUser.Telephone = aUser.Telephone?.Trim() ?? string.Empty;
                aUser.Email = aUser.Email?.Trim() ?? string.Empty;

                return View(new EditAUserProfileViewModel
                {
                    AUser = aUser
                });
            }
            return RedirectToAction("SomethingWentWrong", "Error");
        }

        [HttpPost]
        public ActionResult EditAUserProfile(EditAUserProfileViewModel viewModel, string action)
        {
            if (!ModelState.IsValid)
            {
                viewModel.NewPassword = "";
                viewModel.ConfirmPassword = "";
                return View(viewModel);
            }

            if (action.Equals(Resources.Cancel))
            {
                return RedirectToAction("Index", "User");
            }

            if ((!string.IsNullOrEmpty(viewModel.NewPassword) &&
                 !viewModel.NewPassword.Equals(viewModel.ConfirmPassword))
                ||
                (!string.IsNullOrEmpty(viewModel.ConfirmPassword) &&
                 !viewModel.ConfirmPassword.Equals(viewModel.NewPassword)))
            {
                ModelState.AddModelError("ConfirmPassword", Resources.ConfirmPasswordMustMatchPassword);
                viewModel.NewPassword = "";
                viewModel.ConfirmPassword = "";
                return View(viewModel);
            }

            if (!string.IsNullOrEmpty(viewModel.NewPassword))
            {
                viewModel.AUser.Password = viewModel.NewPassword;
            }

            UserHelper.UpdateAUser(viewModel.AUser);

            return View(viewModel);
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
            var filePath = UploadFile(registerChildViewModel);

            var aUsername = Request.Cookies.Get(CookieHelper.AUserCookie)?.Value;
            var aUser = UserHelper.GetAUserByUsername(aUsername);

            // Save created EndUser to DB
            var newEndUser = new EndUser()
            {
                Username = registerChildViewModel.Username,
                Name = string.IsNullOrEmpty(registerChildViewModel.Name)
                    ? registerChildViewModel.Username
                    : registerChildViewModel.Name,
                PicturePath = filePath,
                AUsers = new List<AUser>()
            };

            newEndUser.AUsers.Add(aUser);
            newEndUser = UserHelper.AddEndUser(newEndUser);

            // Add endUser cookie for newly created End User
            CookieHelper.AddCookie(CookieHelper.EndUserCookie, newEndUser.Id.ToString(), Response);

            return RedirectToAction("EditEndUserProfile");
        }

        [HttpGet]
        public ActionResult EditEndUserProfile(string endUserId)
        {
            if (string.IsNullOrEmpty(endUserId))
            {
                endUserId = Request.Cookies.Get(CookieHelper.EndUserCookie)?.Value;
                if (string.IsNullOrEmpty(endUserId))
                {
                    return RedirectToAction("SomethingWentWrong", "Error");
                }
            }

            var endUserIdInt = -1;
            int.TryParse(endUserId, out endUserIdInt);

            Response.Cookies.Add(new HttpCookie(CookieHelper.EndUserCookie, endUserId));

            var endUser = UserHelper.GetEndUserById(endUserIdInt);

            if (endUser == null)
            {
                return RedirectToAction("SomethingWentWrong", "Error");
            }
            return View(new EditEndUserViewModel
            {
                EndUser = endUser,
                IsSuccessfullySaved = null,
                NewPicture = null
            });
        }

        [HttpPost]
        public ActionResult EditEndUserProfile(EditEndUserViewModel editEndUserViewModel, string action)
        {
            if (action.Equals(Resources.Cancel))
            {
                CookieHelper.RemoveCookie(CookieHelper.EndUserCookie, Response);
                return RedirectToAction("Index", "MetaCommunicator");
            }

            if (!ModelState.IsValid) { return View(editEndUserViewModel); }

            var oldProfPict = editEndUserViewModel.EndUser.PicturePath.Trim();

            if (editEndUserViewModel.NewPicture != null)
            {
                //Save picture on server
                var filePath = UploadFile(new RegisterChildViewModel
                {
                    Picture = editEndUserViewModel.NewPicture,
                    Name = editEndUserViewModel.EndUser.Name.Trim(),
                    Username = editEndUserViewModel.EndUser.Username.Trim()
                });

                editEndUserViewModel.EndUser.PicturePath = filePath;
            }

            var stringEndUserId = Request.Cookies.Get(CookieHelper.EndUserCookie)?.Value;

            int endUserId;
            int.TryParse(stringEndUserId, out endUserId);

            editEndUserViewModel.EndUser.Id = endUserId;

            editEndUserViewModel.IsSuccessfullySaved = UserHelper.UpdateEndUser(editEndUserViewModel.EndUser);

            if (editEndUserViewModel.IsSuccessfullySaved.Value
                && !editEndUserViewModel.EndUser.PicturePath.Equals(oldProfPict)
                && !AppSettings.DefaultProfilePictureFileName.Equals(oldProfPict)
                && editEndUserViewModel.NewPicture != null)
            {
                DriveHelper.DeleteFile(oldProfPict);
            }

            CookieHelper.RemoveCookie(CookieHelper.EndUserCookie, Response);
            return RedirectToAction("Index", "MetaCommunicator");
        }

        [HttpGet]
        public ActionResult EndUserIndex(string endUserId)
        {
            if (!string.IsNullOrEmpty(endUserId))
            {
                CookieHelper.AddCookie(CookieHelper.EndUserCookie, endUserId, Response);
            }
            else
            {
                var endUserCookie = Request.Cookies.Get(CookieHelper.EndUserCookie);
                endUserId = endUserCookie?.Value;
            }

            if (string.IsNullOrEmpty(endUserId))
            {
                return RedirectToAction("NotFound", "Error");
            }

            // TODO: fetch starting point options to display

            return View(new EndUserIndexViewModel { EndUser = UserHelper.GetEndUserById(int.Parse(endUserId)) });
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

        private string UploadFile(RegisterChildViewModel registerChildViewModel)
        {

            try
            {
                var file = registerChildViewModel.Picture;
                var fileExtension = file?.FileName.GetFileExtension();

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

                    var filePath = Path.Combine(Server.MapPath(AppSettings.EndUserProfilePictureDirectory), fileName);

                    registerChildViewModel.Picture.SaveAs(filePath);

                    var uploadedFile = DriveHelper.UploadFile(filePath, AppSettings.ACommunicatorPhotosDriveFolderId);

                    if (uploadedFile != null)
                    {
                        System.IO.File.Delete(filePath);
                    }

                    return (uploadedFile?.Title) ?? AppSettings.DefaultProfilePictureFileName;
                }

            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
            }
            return AppSettings.DefaultProfilePictureFileName;
        }

        /// <summary>
        /// Deprecated;
        /// Uploading files locally - on server.
        /// </summary>
        /// <param name="registerChildViewModel"></param>
        /// <returns></returns>
        private string UploadFileLocally(RegisterChildViewModel registerChildViewModel)
        {

            try
            {
                var file = registerChildViewModel.Picture;
                var fileExtension = file?.FileName.GetFileExtension();

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

                    return Path.Combine(AppSettings.EndUserProfilePictureDirectory, fileName);
                }

            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
            }
            return AppSettings.DefaultProfilePictureFileName;
        }

    }
}