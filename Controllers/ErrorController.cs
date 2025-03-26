using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace MVCDHProject5.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        [Route("ClientError/{StatusCode}")]
        [Route("ServerError")]
        public IActionResult ServerErrorHandler()
        {
            var ExceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ViewBag.ErrorTitle = ExceptionDetails.Error.GetType().Name;
            ViewBag.Path = ExceptionDetails.Path;
            ViewBag.ErrorMessage = ExceptionDetails.Error.Message;
            return View("ServerErrorView");
        }
        public IActionResult ClientErrorHandler(int StatusCode)
        {
            switch (StatusCode)
            {
                case 400:
                    ViewBag.ErrorTitle = "BadRequest";
                    ViewBag.ErrorMessage = "The servercan’treturn a response due toanerroronthe client’s end.";
                    break;
                case 401:
                    ViewBag.ErrorTitle = "Unauthorized or Authorization Required";
                    ViewBag.ErrorMessage = "Returnedbyserverwhenthe targetresource lacks authenticationcredentials.";
                    break;
                case 402:
                    ViewBag.ErrorTitle = "Payment Required";
                    ViewBag.ErrorMessage = "Processingthe requestis notpossible due tolack of requiredfunds.";
                    break;
                case 403:
                    ViewBag.ErrorTitle = "Forbidden";
                    ViewBag.ErrorMessage = "Youare attemptingtoaccess the resource thatyoudon’thave permissionto  view.";
                     break;
                case 404:
                    ViewBag.ErrorTitle = "Not Found";
                    ViewBag.ErrorMessage = "The requestedresource does notexist, andserverdoes notknow if itever  existed.";
                    break;
                case 405:
                    ViewBag.ErrorTitle = "MethodNot Allowed";
                    ViewBag.ErrorMessage = "Hostingserversupports the methodreceived, butthe targetresource doesn’t.";
                    break;
                default:
                    ViewBag.ErrorTitle = "Client Error Occured";
                    ViewBag.ErrorMessage = "There is a Client-Errorin the page, re-check the input you supplied.";
                    break;
            }
            return View("ClientErrorView");
        }
    }
}
