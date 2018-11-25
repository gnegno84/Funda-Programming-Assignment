using Autofac;
using Funda.ProgrammingAssignment.ServiceProxy;

namespace Funda.ProgrammingAssignment.Console.Modules
{
    //This module injects a more efficient, parallelized implementation of the repository that is going to ask to the Funda API the data
    public class ParallelApiIntegrationServicesModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ParallellizedFundaApiBasedRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();

        }
    }
}