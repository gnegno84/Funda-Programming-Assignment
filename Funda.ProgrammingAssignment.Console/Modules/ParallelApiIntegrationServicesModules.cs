using Autofac;
using Funda.ProgrammingAssignment.ServiceProxy;

namespace Funda.ProgrammingAssignment.Console.Modules
{
    public class ParallelApiIntegrationServicesModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ParallellizedFundaApiBasedRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();

        }
    }
}