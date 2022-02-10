using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MVCApp.Controllers
{
    public class BaseController : Controller
    {
        // gọi function này trước khi controller được gọi
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var sessions = context.HttpContext.Session.GetString("Token");
            if (sessions == null)
            {
                context.Result = new RedirectToActionResult("Login", "User", null);
            }
            base.OnActionExecuting(context);
        }
    }
}
