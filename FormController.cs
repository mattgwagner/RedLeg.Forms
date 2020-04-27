using Microsoft.AspNetCore.Mvc;

namespace RedLeg.Forms
{
    public class FormController : Controller
    {
        [HttpGet("/")]
        public ActionResult Index()
        {
            return Content("Hello world!", "text/plain");
        }
    }
}
