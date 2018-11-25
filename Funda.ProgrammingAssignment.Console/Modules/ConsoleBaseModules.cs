using System;
using Autofac;
using Funda.ProgrammingAssignment.Console.ConsoleDumpers;
using Funda.ProgrammingAssignment.Console.ConsoleDumpers.RequestStatusUpdater;
using Funda.ProgrammingAssignment.Console.ConsoleDumpers.TableDumper;
using Funda.ProgrammingAssignment.Console.Model;
using Funda.ProgrammingAssignment.Domain.BL;
using Funda.ProgrammingAssignment.Domain.Services.ResultMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using Microsoft.Extensions.Logging.Console;
using Module = Autofac.Module;

namespace Funda.ProgrammingAssignment.Console.Modules
{
    //This is the "main" module of the application. The one that injects the common logic for all the different scenarios
    public class ConsoleBaseModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RealEstateAgentsBl>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<ResultMapper>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterInstance(new LoggerFactory())
                .As<ILoggerFactory>();
            builder.Register(context => context.Resolve<ILoggerFactory>()
                    .AddConsole(context.Resolve<IConfiguration>().GetSection("Logging"))
                    .CreateLogger<ConsoleLogger>())
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<SimpleDumperProgressBarRequestStatusUpdater>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<JsonRealEstateAgentSalesDataConsoleDumper>().Keyed<IRealEstatesAgentSalesDataConsoleDumper>(DumperTypeEnum.Json).InstancePerLifetimeScope();
            builder.RegisterType<TableRealEstateAgentSalesDataConsoleDumper>().Keyed<IRealEstatesAgentSalesDataConsoleDumper>(DumperTypeEnum.Table).InstancePerLifetimeScope();
            builder.RegisterType<RealEstateAgentSalesDataTableCreator>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
