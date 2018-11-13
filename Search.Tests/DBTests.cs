using System;
using System.Data.SqlClient;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Search.Models;

namespace Search.Tests
{
    [TestClass]
    public class DBTests
    {
        protected SearchContext SearchContext;

        [TestInitialize]
        public void Initialize()
        {
            string script = File.ReadAllText("Search.sql");

            string connectionString = "data source=(LocalDB)\\MSSQLLocalDB;attachdbfilename=|DataDirectory|\\SearchTest.mdf;integrated security=True;connect timeout=30;MultipleActiveResultSets=True;App=EntityFramework";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand(script, connection);
                sqlCommand.ExecuteNonQuery();
                connection.Close();
            }

            SearchContext = new SearchContext();
        }

        [TestMethod]
        public void AddNewSearchEngine()
        {
            SearchEngine google = new SearchEngine()
            {
                Name = "Google",
                Domain = "https://www.google.com",
                URL = "https://www.google.com/search?q="
            };
            SearchEngine searchEngine = AddNewSearchEngine(google);
            Assert.IsNotNull(searchEngine);
            Assert.AreEqual(searchEngine.Name, google.Name);
            Assert.AreEqual(searchEngine.Domain, google.Domain);
            Assert.AreEqual(searchEngine.URL, google.URL);
        }

        private SearchEngine AddNewSearchEngine(SearchEngine google)
        {
            SearchContext.SearchEngines.Add(google);
            SearchContext.SaveChanges();

            return SearchContext.SearchEngines.Find(1);
        }

        [TestMethod]
        public void AddNewSearchQuery()
        {
            SearchEngine google = new SearchEngine()
            {
                Name = "Google",
                Domain = "https://www.google.com",
                URL = "https://www.google.com/search?q="
            };
            google = AddNewSearchEngine(google);
            SearchQuery searchQueryBefore = new SearchQuery()
            {
                SearchDate = DateTime.Now,
                SearchEngineId = google.Id,
                Query = "Test"
            };
            SearchQuery searchQueryAfter = AddNewSearchQuery(searchQueryBefore);
            Assert.IsNotNull(searchQueryAfter);
            Assert.AreEqual(searchQueryAfter.Query, searchQueryBefore.Query);
            Assert.AreEqual(searchQueryAfter.SearchDate, searchQueryBefore.SearchDate);
            Assert.AreEqual(searchQueryAfter.SearchEngineId, searchQueryBefore.SearchEngineId);
        }

        private SearchQuery AddNewSearchQuery(SearchQuery searchQueryBefore)
        {
            SearchContext.SearchQueries.Add(searchQueryBefore);
            SearchContext.SaveChanges();

            return SearchContext.SearchQueries.Find(1);
        }

        [TestMethod]
        public void AddNewSearchResult()
        {
            SearchEngine google = new SearchEngine()
            {
                Name = "Google",
                Domain = "https://www.google.com",
                URL = "https://www.google.com/search?q="
            };
            google = AddNewSearchEngine(google);
            SearchQuery searchQuery = new SearchQuery()
            {
                SearchDate = DateTime.Now,
                SearchEngineId = google.Id,
                Query = "Test"
            };
            searchQuery = AddNewSearchQuery(searchQuery);

            SearchResult searchResultBefore = new SearchResult()
            {
                Text = "http://test.com",
                SearchQueryId = searchQuery.Id,
                URL = "http://test.com"
            };

            SearchResult searchResultAfter = AddNewSearchResult(searchResultBefore);
            Assert.IsNotNull(searchResultBefore);
            Assert.AreEqual(searchResultBefore.Text, searchResultAfter.Text);
            Assert.AreEqual(searchResultBefore.SearchQueryId, searchResultAfter.SearchQueryId);
            Assert.AreEqual(searchResultBefore.URL, searchResultAfter.URL);
        }

        private SearchResult AddNewSearchResult(SearchResult searchResultBefore)
        {
            SearchContext.SearchResults.Add(searchResultBefore);
            SearchContext.SaveChanges();

            return SearchContext.SearchResults.Find(1);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (SearchContext != null)
            {
                SearchContext.Dispose();
                SearchContext = null;
            }
        }
    }
}
