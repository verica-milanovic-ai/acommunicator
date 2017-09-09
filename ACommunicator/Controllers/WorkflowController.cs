using System.Linq;
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

            return PartialView("Options", optionItemViewModelList);
        }

        [HttpGet]
        public ActionResult SelectOption(int optionId)
        {
            var option = OptionHelper.GetOption(optionId);

            if (option == null) return RedirectToAction("SomethingWentWrong", "Error");
            switch (option.Level)
            {
                case 1:
                case 2:
                    {
                        return RedirectToAction("Options", new { optionId = option.Id });
                    }
                case 3:
                    {
                        var optionMediaModel = OptionHelper.GetOption(option);
                        return PartialView("FinalOption", optionMediaModel);
                    }
                default:
                    return RedirectToAction("SomethingWentWrong", "Error");
            }
        }
    }
}
