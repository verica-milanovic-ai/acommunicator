using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ACommunicator.Helpers.Google;
using ACommunicator.Models;

namespace ACommunicator.Controllers
{
    public class WorkflowController : Controller
    {
        [HttpGet]
        public ActionResult Options(string optionId)
        {
            var file = DriveHelper.GetFiles("default").First();

            var fileEmbedLink = file.ThumbnailLink;


            return View("Options", new List<OptionViewModel> { new OptionViewModel { ImageUrl = fileEmbedLink, Text = "default option", OptionId = "1" } });
        }
    }
}
