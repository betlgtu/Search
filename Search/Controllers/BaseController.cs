using Search.Models;
using System.Web.Mvc;

namespace Search.Controllers
{
    public abstract class BaseController : Controller
    {
        protected SearchContext searchContext = new SearchContext();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                searchContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}