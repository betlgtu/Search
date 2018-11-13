using Search.App_Start;
using System.Web.Mvc;

namespace Search
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            IConfigurator[] configurators = new IConfigurator[]
            {
                new RoutesConfigurator(),
                new BundlesConfigurator()
            };

            foreach (IConfigurator configurator in configurators)
            {
                configurator.Configure();
            }
        }
    }
}
