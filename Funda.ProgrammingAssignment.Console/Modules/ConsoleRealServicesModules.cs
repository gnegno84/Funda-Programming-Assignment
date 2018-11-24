using Autofac;
using Funda.ProgrammingAssignment.ServiceProxy;
using Funda.ProgrammingAssignment.ServiceProxy.Services.ApiConfigurationParser;
using Funda.ProgrammingAssignment.ServiceProxy.Services.DtoMapper;
using Funda.ProgrammingAssignment.ServiceProxy.Services.RequestBuilder;
using Funda.ProgrammingAssignment.ServiceProxy.Services.ResilienceWrapper;
using Funda.ProgrammingAssignment.ServiceProxy.Services.SearchTermsFormatter;

namespace Funda.ProgrammingAssignment.Console.Modules
{
    public class ConsoleRealServicesModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FundaApiBasedRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<FundaApiProxyService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<SimpleDtoMapper>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<FixedApiConfigurationParser>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<RestRequestsBuilder>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<PollyResiliencePolicyWrapper>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<SearchTermsFormatter>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}