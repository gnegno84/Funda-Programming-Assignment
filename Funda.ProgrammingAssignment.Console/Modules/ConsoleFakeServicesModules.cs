using Autofac;
using Funda.ProgrammingAssignment.Domain.Services;

namespace Funda.ProgrammingAssignment.Console.Modules
{
    public class ConsoleFakeServicesModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FakePropertiesRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}