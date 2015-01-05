
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MyFixIt.Logging;
using MyFixIt.Persistence;

namespace MyFixIt
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            DependenciesConfig.RegisterDependencies();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            PhotoService photoService = new PhotoService(new Logger());
            photoService.CreateAndConfigureAsync();

            DbConfiguration.SetConfiguration(new EfConfiguration());
        }
    }
}
