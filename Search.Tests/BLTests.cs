using Microsoft.VisualStudio.TestTools.UnitTesting;
using Search.Models;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Search.Tests
{
    [TestClass]
    public class BLTests
    {
        private SearchEngine Google = new SearchEngine()
        {
            Name = "Google",
            URL = "https://www.google.com/search?q=",
            Domain = "https://www.google.com"
        };

        private SearchEngine Yandex = new SearchEngine()
        {
            Name = "Yandex",
            URL = "https://www.yandex.ru/search/?lr=193&text=",
            Domain = "https://www.yandex.ru"
        };

        private SearchEngine Bing = new SearchEngine()
        {
            Name = "Bing",
            URL = "https://www.bing.com/search?q=",
            Domain = "https://www.bing.com"
        };

        [TestMethod]
        public void SearchOne()
        {
            SearchExecutor searchExecutor = new SearchExecutor();
            
            Task<QueryResult> queryResult = searchExecutor.SearchAsync(Google, "Test", new CancellationToken());
            queryResult.Wait();

            Assert.IsNotNull(queryResult.Result);
            Assert.IsNotNull(queryResult.Result.SearchResult);
        }

        [TestMethod]
        public void SearchMany()
        {
            SearchExecutor searchExecutor = new SearchExecutor();
            SearchEngine[] searchEngines = new SearchEngine[] { Yandex, Google };

            Task<QueryResult> queryResult = searchExecutor.SearchAsync(searchEngines, "Test");
            queryResult.Wait();

            Assert.IsNotNull(queryResult.Result);
            Assert.IsNotNull(queryResult.Result.SearchResult);
        }

        [TestMethod]
        public void ParserTest()
        {
            SearchExecutor searchExecutor = new SearchExecutor();

            Task<QueryResult> queryResult = searchExecutor.SearchAsync(Yandex, "МСФО", new CancellationToken());
            queryResult.Wait();

            Assert.IsNotNull(queryResult.Result);
            Assert.IsNotNull(queryResult.Result.SearchResult);

            AnchorParser parser = new AnchorParser();
            Link[] links = parser.ParseLinks(queryResult.Result.SearchEngine.Domain, queryResult.Result.SearchResult);
            Assert.IsNotNull(links);

            foreach (Link link in links)
            {
                Debug.WriteLine(link.URL);
            }
            Assert.IsTrue(links.Length > 10);
        }
    }
}
