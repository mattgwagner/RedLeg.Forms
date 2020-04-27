using Microsoft.AspNetCore.Mvc;
using System;

namespace RedLeg.Forms
{
    public class FormController : Controller
    {
        [HttpGet("/")]
        public ActionResult Index()
        {
            return Content("Hello world!", "text/plain");
        }

        public ActionResult DA4856()
        {
            throw new NotImplementedException();
        }

        public ActionResult DA5500()
        {
            throw new NotImplementedException();
        }

        public ActionResult DA5501()
        {
            throw new NotImplementedException();
        }

        public ActionResult DA705()
        {
            throw new NotImplementedException();
        }

        public ActionResult DA3749()
        {
            throw new NotImplementedException();
        }
    }
}
