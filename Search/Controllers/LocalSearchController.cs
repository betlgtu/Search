using Search.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Search.Controllers
{
    public class LocalSearchController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ActionName(nameof(Index))]
        [ValidateAntiForgeryToken]
        public ActionResult Search(string query)
        {
            if (string.IsNullOrEmpty(query))
                return View(nameof(Index), null);

            if (string.IsNullOrEmpty(query.Trim()))
                return View(nameof(Index), null);

            SearchQuery searchQuery = searchContext.SearchQueries.FirstOrDefault(sq => sq.Query == query);

            if (searchQuery == null)
                return View(nameof(Index), null);

            SearchResult[] searchResults = searchContext.SearchResults.Where(sr => sr.SearchQueryId == searchQuery.Id).ToArray();
            return View(nameof(Index), searchResults);
        }

    }
}