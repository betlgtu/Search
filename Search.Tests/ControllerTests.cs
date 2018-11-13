using Microsoft.VisualStudio.TestTools.UnitTesting;
using Search.Controllers;
using Search.Models;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Search.Tests
{
    [TestClass]
    public class ControllerTests
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
        public void MainIndexTest()
        {
            MainController mainController = new MainController();
            ViewResult viewResult = mainController.Index() as ViewResult;
            Assert.IsNotNull(viewResult);
        }

        [TestMethod]
        public void SearchEngineIndexTest()
        {
            SearchEngineController searchEngineController = new SearchEngineController();
            Task<ActionResult> task = searchEngineController.Index();
            task.Wait();
            ActionResult actionResult = task.Result;
            ViewResult viewResult = actionResult as ViewResult;
            Assert.IsNotNull(viewResult);
        }

        [TestMethod  ]
        public void SearchEngineDetailsTest()
        {
            SearchEngineController searchEngineController = new SearchEngineController();
            SearchEngine google = new SearchEngine()
            {
                Name = "Google",
                Domain = "https://www.google.com",
                URL = "https://www.google.com/search?q="
            };
            SearchContext.SearchEngines.Add(google);
            SearchContext.SaveChanges();

            Task<ActionResult> task = searchEngineController.Details(google.Id);
            task.Wait();
            ActionResult actionResult = task.Result;
            ViewResult viewResult = actionResult as ViewResult;
            Assert.IsNotNull(viewResult);
        }

        [TestMethod]
        public void SearchEngineEditGetTest()
        {
            SearchEngineController searchEngineController = new SearchEngineController();
            SearchEngine google = new SearchEngine()
            {
                Name = "Google",
                Domain = "https://www.google.com",
                URL = "https://www.google.com/search?q="
            };
            SearchContext.SearchEngines.Add(google);
            SearchContext.SaveChanges();

            Task<ActionResult> task = searchEngineController.Edit(google.Id);
            task.Wait();
            ActionResult actionResult = task.Result;
            ViewResult viewResult = actionResult as ViewResult;
            Assert.IsNotNull(viewResult);
        }

        [TestMethod]
        public void SearchEngineEditPostTest()
        {
            SearchEngineController searchEngineController = new SearchEngineController();
            SearchEngine google = new SearchEngine()
            {
                Name = "Google",
                Domain = "https://www.google.com",
                URL = "https://www.google.com/search?q="
            };
            SearchContext.SearchEngines.Add(google);
            SearchContext.SaveChanges();

            google.Name = "Yandex";
            Task<ActionResult> task = searchEngineController.Edit(google);
            task.Wait();
            ActionResult actionResult = task.Result;
            RedirectToRouteResult redirectToRouteResult = actionResult as RedirectToRouteResult;
            Assert.IsNotNull(redirectToRouteResult);
            Assert.IsTrue(google.Id > 0);
            SearchEngine result = SearchContext.SearchEngines.Find(google.Id);
            Assert.AreEqual(google.Name, result.Name);
        }

        [TestMethod]
        public void SearchEngineCreateGetTest()
        {
            SearchEngineController searchEngineController = new SearchEngineController();
            ActionResult actionResult = searchEngineController.Create();
            ViewResult viewResult = actionResult as ViewResult;
            Assert.IsNotNull(viewResult);
        }

        [TestMethod]
        public void SearchEngineCreatePostTest()
        {
            SearchEngineController searchEngineController = new SearchEngineController();
            SearchEngine google = new SearchEngine()
            {
                Name = "Google",
                Domain = "https://www.google.com",
                URL = "https://www.google.com/search?q="
            };

            Task<ActionResult> task = searchEngineController.Create(google);
            task.Wait();
            ActionResult actionResult = task.Result;
            RedirectToRouteResult redirectToRouteResult = actionResult as RedirectToRouteResult;
            Assert.IsNotNull(redirectToRouteResult);
            Assert.IsTrue(google.Id > 0);
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
