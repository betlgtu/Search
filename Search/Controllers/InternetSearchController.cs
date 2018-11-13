using Search.Models;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Search.Controllers
{
    public class InternetSearchController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ActionName(nameof(Index))]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
                return View(nameof(Index), null);

            if (string.IsNullOrEmpty(query.Trim()))
                return View(nameof(Index), null);

            SearchEngine[] searchEngines = await searchContext.SearchEngines.ToArrayAsync();
            SearchExecutor searchExecutor = new SearchExecutor();
            QueryResult queryResult = await searchExecutor.SearchAsync(searchEngines, query);
            AnchorParser anchorParser = new AnchorParser();
            Link[] links = anchorParser.ParseLinks(queryResult.SearchEngine.Domain, queryResult.SearchResult);

            if (links.Length > 0)
            {
                SearchQuery searchQuery = searchContext.SearchQueries.FirstOrDefault(sq => sq.Query == query);
                if (searchQuery != null)
                {
                    RemoveRelatedSearchResults(searchQuery);
                }
                else
                {
                    searchQuery = new SearchQuery() { Query = query, SearchDate = DateTime.Now, SearchEngineId = queryResult.SearchEngine.Id };
                    searchContext.SearchQueries.Add(searchQuery);
                    await searchContext.SaveChangesAsync();
                }

                if (links.Length > 10)
                    links = links.Take(10).ToArray();

                AddSearchResults(queryResult.SearchEngine, searchQuery, links);
            }
            return View("Index", links);
        }

        private void AddSearchResults(SearchEngine searchEngine, SearchQuery searchQuery, Link[] links)
        {
            using (var transaction = searchContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (Link link in links)
                    {
                        SearchResult searchResult = new SearchResult()
                        {
                            Text = link.Title,
                            URL = link.URL,
                            SearchQueryId = searchQuery.Id
                        };
                        searchContext.SearchResults.Add(searchResult);
                    }

                    searchContext.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }

        private void RemoveRelatedSearchResults(SearchQuery searchQuery)
        {
            SearchResult[] searchResults = searchContext.SearchResults.Where(sr => sr.SearchQueryId == searchQuery.Id).ToArray();

            using (var transaction = searchContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (SearchResult searchResult in searchResults)
                    {
                        searchContext.SearchResults.Remove(searchResult);
                    }

                    searchContext.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }
    }
}