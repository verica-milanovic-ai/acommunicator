using System.Web.Mvc;
using ACommunicator.Models;
using ACommunicator.Properties;

namespace ACommunicator.Controllers
{
    public class ErrorController : BaseController
    {
        [HttpGet]
        public ActionResult NotFound()
        {
            return View("Error", new ErrorViewModel
            {
                Message = Resources.NotFoundMessage
            });
        }

        [HttpGet]
        public ActionResult SomethingWentWrong()
        {
            return View(new ErrorViewModel
            {
                Message = Resources.SomethingWentWrongMessage,
                Status = 500
            });
        }

        [HttpGet]
        public ActionResult BadRequest()
        {
            return View("Error", new ErrorViewModel
            {
                Message = Resources.BadRequestMessage
            });
        }
    }
}