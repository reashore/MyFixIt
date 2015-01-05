using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using MyFixIt.Logging;
using MyFixIt.Persistence;

namespace MyFixIt
{
    public class DependenciesConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<Logger>().As<ILogger>().SingleInstance();
            builder.RegisterType<FixItTaskRepository>().As<IFixItTaskRepository>();
            builder.RegisterType<PhotoService>().As<IPhotoService>().SingleInstance();
            builder.RegisterType<FixItQueueManager>().As<IFixItQueueManager>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}