using Search.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Search.Controllers
{
    public abstract class GenericController<T> : BaseController
        where T : BaseEntity
    {
        protected abstract DbSet<T> DbSet { get; }
        protected virtual int PageSize => 10;
        
        public async Task<ActionResult> Index(int skip = 0)
        {
            List<T> entities = await DbSet.OrderBy(e => e.Id)
                                          .Skip(skip)
                                          .Take(PageSize)
                                          .ToListAsync();
            return View(entities);
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T entity = await DbSet.FindAsync(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            T entity = await DbSet.FindAsync(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T entity = await DbSet.FindAsync(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> DeleteConfirmed(int id)
        {
            T entity = await DbSet.FindAsync(id);
            DbSet.Remove(entity);
            await searchContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult Create() => View();

        [HttpGet]
        public ActionResult Table() => PartialView();
    }
}