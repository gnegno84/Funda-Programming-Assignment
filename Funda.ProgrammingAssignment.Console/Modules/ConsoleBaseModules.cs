using Autofac;
using Funda.ProgrammingAssignment.Console.ConsoleDumpers;
using Funda.ProgrammingAssignment.Console.ConsoleDumpers.TableDumper;
using Funda.ProgrammingAssignment.Console.Model;
using Funda.ProgrammingAssignment.Domain.BL;
using Funda.ProgrammingAssignment.Domain.Services.ResultMapper;
using Module = Autofac.Module;

namespace Funda.ProgrammingAssignment.Console.Modules
{
    public class ConsoleBaseModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RealEstateAgentsBl>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<ResultMapper>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<JsonRealEstateAgentSalesDataConsoleDumper>().Keyed<IRealEstatesAgentSalesDataConsoleDumper>(DumperTypeEnum.Json).InstancePerLifetimeScope();
            builder.RegisterType<TableRealEstateAgentSalesDataConsoleDumper>().Keyed<IRealEstatesAgentSalesDataConsoleDumper>(DumperTypeEnum.Table).InstancePerLifetimeScope();
            builder.RegisterType<RealEstateAgentSalesDataTableCreator>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
