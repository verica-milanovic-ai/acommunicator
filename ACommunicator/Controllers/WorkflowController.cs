using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACommunicator.Helpers;
using ACommunicator.Models;

namespace ACommunicator.Controllers
{
    public class WorkflowController : Controller
    {
        [HttpGet]
        public ActionResult Options(int optionId)
        {
            var optionList = OptionHelper.GetChildOptionList(optionId);
            var optionItemViewModelList = optionList.Select(OptionHelper.GetOption)
                                            .Select(optionMediaModel =>
                                                new OptionItemViewModel(optionMediaModel, "SelectOption", "Workflow"))
                                            .ToList();
            var endUserId = 0;
            int.TryParse(Request.Cookies.Get(CookieHelper.EndUserCookie)?.Value, out endUserId);
            var endUser = UserHelper.GetEndUserById(endUserId);

            var model = new EndUserIndexViewModel
            {
                Options = optionItemViewModelList,
                EndUser = endUser,
                SelectedOptions = GetSelectedOptions()
            };

            return View("../User/EndUserIndex", model);
        }
        
        [HttpGet]
        public ActionResult SelectOption(int optionId)
        {
            var option = OptionHelper.GetOption(optionId);

            if (option == null) return RedirectToAction("SomethingWentWrong", "Error");

            if (!IsSelectedOptionInCookie(optionId))
            {
                AddOptionToCookie(optionId);
            }
            else
            {
                RemoveOptionsFromCookie(optionId);
            }

            if (option.Level > 0 && option.Level < 5)
            {
                return RedirectToAction("Options", new { optionId = option.Id });
            }
            return RedirectToAction("SomethingWentWrong", "Error");
        }

        /// <summary>
        /// Add optionId to 
        /// </summary>
        /// <param name="optionId"></param>
        private void AddOptionToCookie(int optionId)
        {
            if (Request.Cookies.Get(CookieHelper.SelectedOptionsCookie) == null
                || string.IsNullOrEmpty(Request.Cookies.Get(CookieHelper.SelectedOptionsCookie)?.Value))
            {
                CookieHelper.AddCookie(CookieHelper.SelectedOptionsCookie, optionId.ToString(), Response);
            }
            else
            {
                var selectedOptions = Request.Cookies.Get(CookieHelper.SelectedOptionsCookie)?.Value;

                selectedOptions = string.IsNullOrEmpty(selectedOptions)
                    ? string.Concat(selectedOptions, optionId.ToString())
                    : string.Concat(selectedOptions, "," + optionId);
                Response.Cookies.Set(new HttpCookie(CookieHelper.SelectedOptionsCookie, selectedOptions));
            }
        }

        /// <summary>
        /// Removes all optionIds after passed optionId
        /// </summary>
        /// <param name="optionId">the last option that needs to stay on selected options stack</param>
        private void RemoveOptionsFromCookie(int optionId)
        {
            var selectedOptions = Request.Cookies.Get(CookieHelper.SelectedOptionsCookie)?.Value;
            if (string.IsNullOrEmpty(selectedOptions)) return;
            var positionOfCookie = selectedOptions.IndexOf(optionId.ToString(), StringComparison.Ordinal);

            // if there is optionId in cookie and it's not at the beginning
            // then remove all characters after selected optionId 
            if (positionOfCookie >= 0)
            {
                positionOfCookie += optionId.ToString().Length;
                selectedOptions = selectedOptions.Remove(positionOfCookie);
                Response.Cookies.Set(new HttpCookie(CookieHelper.SelectedOptionsCookie, selectedOptions));
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>optionId of last selected option</returns>
        private int GetLastSelectedOptionId()
        {
            var returnValue = 0;
            var selectedOptions = Request.Cookies.Get(CookieHelper.SelectedOptionsCookie)?.Value;
            if (string.IsNullOrEmpty(selectedOptions)) return returnValue;

            var selectedOptionArray = selectedOptions.Split(',');
            int.TryParse(selectedOptionArray[selectedOptionArray.Length - 1], out returnValue);

            return returnValue;
        }

        private bool IsSelectedOptionInCookie(int optionId)
        {
            var selectedOptions = Request.Cookies.Get(CookieHelper.SelectedOptionsCookie)?.Value;
            if (string.IsNullOrEmpty(selectedOptions))
                return false;
            var selectedOptionsArray = selectedOptions.Split(',');
            return selectedOptionsArray.Contains(optionId.ToString());
        }
        

        private List<OptionItemViewModel> GetSelectedOptions()
        {
            var retVal = new List<OptionItemViewModel>();
            var selectedOptions = Request.Cookies.Get(CookieHelper.SelectedOptionsCookie)?.Value;

            if (string.IsNullOrEmpty(selectedOptions))
                return retVal;
            var selectedOptionsArray = selectedOptions.Split(',');
            foreach (var option in selectedOptionsArray)
            {
                var optionId = 0;
                int.TryParse(option, out optionId);

                var optionObj = OptionHelper.GetOption(optionId);

                retVal.Add(new OptionItemViewModel(OptionHelper.GetOption(optionObj), "SelectOption", "Workflow"));
            }
            return retVal;
        }
    }
}
