using System.Web.Mvc;
using log4net;

namespace ACommunicator.Controllers
{
    public abstract class BaseController : Controller
    {
        public static readonly ILog log = LogManager.GetLogger("LogInfo");

        protected override void OnException(ExceptionContext filterContext)
        {
            log.Error(filterContext.Exception.ToString());
            base.OnException(filterContext);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //var request = filterContext.RequestContext.HttpContext.Request.HttpMethod 
            //    + ": " + filterContext.ActionDescriptor.ControllerDescriptor.ControllerName 
            //    + "Controller/" + filterContext.ActionDescriptor.ActionName 
            //    + "; params: " + JsonConvert.SerializeObject(filterContext.ActionParameters);

            //log.Debug(request);

            //base.OnActionExecuting(filterContext);
        }

    }
}