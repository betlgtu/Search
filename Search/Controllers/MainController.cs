using System.Web.Mvc;

namespace Search.Controllers
{
    public class MainController : Controller, IIndexController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}