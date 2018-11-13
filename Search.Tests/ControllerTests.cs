using Microsoft.VisualStudio.TestTools.UnitTesting;
using Search.Controllers;
using Search.Models;
using System;
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
            IndexTest<MainController>();
        }

        [TestMethod]
        public void SearchEngineIndexTest()
        {
            using (SearchEngineController searchEngineController = new SearchEngineController())
            {
                Task<ActionResult> task = searchEngineController.Index();
                task.Wait();
                ActionResult actionResult = task.Result;
                ViewResult viewResult = actionResult as ViewResult;
                Assert.IsNotNull(viewResult);
            }
        }

        [TestMethod  ]
        public void SearchEngineDetailsTest()
        {
            using (SearchEngineController searchEngineController = new SearchEngineController())
            {
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
        }

        [TestMethod]
        public void SearchEngineEditGetTest()
        {
            using (SearchEngineController searchEngineController = new SearchEngineController())
            {
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
        }

        [TestMethod]
        public void SearchEngineEditPostTest()
        {
            using (SearchEngineController searchEngineController = new SearchEngineController())
            {
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
        }

        [TestMethod]
        public void SearchEngineCreateGetTest()
        {
            using (SearchEngineController searchEngineController = new SearchEngineController())
            {
                ActionResult actionResult = searchEngineController.Create();
                ViewResult viewResult = actionResult as ViewResult;
                Assert.IsNotNull(viewResult);
            }
        }

        [TestMethod]
        public void SearchEngineCreatePostTest()
        {
            using (SearchEngineController searchEngineController = new SearchEngineController())
            {
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
        }

        [TestMethod]
        public void InternetSearchIndexGetTest()
        {
            IndexTest<InternetSearchController>();
        }

        [TestMethod]
        public void InternetSearchIndexPostEmptyDBTest()
        {
            using (InternetSearchController internetSearchController = new InternetSearchController())
            {
                Task<ActionResult> task = internetSearchController.Search("Test");
                task.Wait();
                ActionResult actionResult = task.Result;
                ViewResult viewResult = actionResult as ViewResult;
                Assert.IsNotNull(viewResult);
            }
        }

        [TestMethod]
        public void InternetSearchIndexPostTest()
        {
            using (SearchContext searchContext = new SearchContext())
            {
                SearchEngine google = new SearchEngine()
                {
                    Name = "Google",
                    Domain = "https://www.google.com",
                    URL = "https://www.google.com/search?q="
                };
                SearchEngine bing = new SearchEngine()
                {
                    Name = "Bing",
                    Domain = "https://www.bing.com",
                    URL = "https://www.bing.com/search?q="
                };
                searchContext.SearchEngines.Add(google);
                searchContext.SearchEngines.Add(bing);
                searchContext.SaveChanges();
            }

            using (InternetSearchController internetSearchController = new InternetSearchController())
            {
                Task<ActionResult> task = internetSearchController.Search("Test");
                task.Wait();
                ActionResult actionResult = task.Result;
                ViewResult viewResult = actionResult as ViewResult;
                Assert.IsNotNull(viewResult);
                Assert.IsNotNull(viewResult.Model);
            }
        }

        [TestMethod]
        public void LocalSearchIndexGetTest()
        {
            IndexTest<LocalSearchController>();
        }

        [TestMethod]
        public void LocalSearchIndexPostTest()
        {
            using (LocalSearchController localSearchController = new LocalSearchController())
            {
                ActionResult actionResult = localSearchController.Search("Test");
                ViewResult viewResult = actionResult as ViewResult;
                Assert.IsNotNull(viewResult);
            }
        }

        private void IndexTest<T>() where T : Controller, IIndexController, new()
        {
            using (T controller = new T())
            {
                ActionResult actionResult = controller.Index();
                ViewResult viewResult = actionResult as ViewResult;
                Assert.IsNotNull(viewResult);
            }
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
