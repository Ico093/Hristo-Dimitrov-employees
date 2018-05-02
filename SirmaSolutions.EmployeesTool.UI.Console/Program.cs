using Autofac;
using SirmaSolutions.EmployeesTool.BLL.Selectors;
using SirmaSolutions.EmployeesTool.BLL.Selectors.Interfaces;
using SirmaSolutions.EmployeesTool.BLL.TextParsers;
using SirmaSolutions.EmployeesTool.BLL.TextParsers.Interfaces;

namespace SirmaSolutions.EmployeesTool.UI.Console
{
    class Program
    {
        static IContainer BuildContainer(ContainerBuilder builder)
        {
            builder.RegisterType<Application>().AsSelf();
            builder.RegisterType<JobHistoryListParser>().As<IJobHistoryTextParser>();
            builder.RegisterType<CommonProjectsCouplesSelector>().As<ICommonProjectsCouplesSelector>();

            return builder.Build();
        }

        static void Main(string[] args)
        {
            IContainer container = BuildContainer(new ContainerBuilder());

            using (ILifetimeScope scope = container.BeginLifetimeScope())
            {
                Application application = scope.Resolve<Application>();
                application.Run();
            }
        }
    }
}
