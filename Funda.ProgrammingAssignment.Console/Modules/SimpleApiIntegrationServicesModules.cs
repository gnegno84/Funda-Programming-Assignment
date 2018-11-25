using Autofac;
using Funda.ProgrammingAssignment.ServiceProxy;

namespace Funda.ProgrammingAssignment.Console.Modules
{
    public class SimpleApiIntegrationServicesModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FundaApiBasedRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();

        }
    }
}