using System.Web.Optimization;

namespace Search.App_Start
{
    public class BundlesConfigurator : IConfigurator
    {
        public void Configure()
        {
            BundleCollection bundles = BundleTable.Bundles;

            BundleTable.EnableOptimizations = true;

            bundles.Add(new ScriptBundle("~/Scripts/jquery").Include(
                        "~/Scripts/jquery-{version}.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/popper").Include(
                        "~/Scripts/popper.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/master.min.css"));
        }
    }
}