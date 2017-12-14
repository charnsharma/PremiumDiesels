using System.Web.Mvc;

namespace PremiumDiesel.Web.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            ViewBag.Error = Request.Params.Get("message");
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}