using Search.Models;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;

namespace Search.Controllers
{
    public class SearchEngineController : GenericController<SearchEngine>
    {
        protected override DbSet<SearchEngine> DbSet => searchContext.SearchEngines;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name,URL,Domain")] SearchEngine searchEngine)
        {
            if (ModelState.IsValid)
            {
                DbSet.Add(searchEngine);
                await searchContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(searchEngine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,URL,Domain")] SearchEngine searchEngine)
        {
            if (ModelState.IsValid)
            {
                var entry = searchContext.Entry(searchEngine);
                entry.State = EntityState.Modified;
                await searchContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(searchEngine);
        }
    }
}