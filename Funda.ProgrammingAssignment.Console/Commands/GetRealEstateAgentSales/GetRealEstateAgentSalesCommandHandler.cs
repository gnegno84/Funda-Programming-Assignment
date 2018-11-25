using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Funda.ProgrammingAssignment.Console.ConsoleDumpers.TableDumper;
using Funda.ProgrammingAssignment.Console.Modules;
using Funda.ProgrammingAssignment.Domain.BL;
using Microsoft.Extensions.Configuration;

namespace Funda.ProgrammingAssignment.Console.Commands.GetRealEstateAgentSales
{
    class GetRealEstateAgentSalesCommandHandler
    {
        public static int RunGetRealEstateAgentSales(GetRealEstateAgentSalesCommandOptions opts, IConfiguration configuration)
        {
            Task.Run(() => GetTopRealEstateAgentSalesAsync(opts, configuration)).Wait();
            return 0;
        }

        private static async Task GetTopRealEstateAgentSalesAsync(GetRealEstateAgentSalesCommandOptions opts,
            IConfiguration configuration)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<ConsoleBaseModules>();
            builder.RegisterInstance(configuration).AsImplementedInterfaces().SingleInstance();
            RegisterServiceModules(opts, builder);

            var container = builder.Build();

            using (var context = container.BeginLifetimeScope())
            {
                var dataDumper = context.ResolveKeyed<IRealEstatesAgentSalesDataConsoleDumper>(opts.OutputType);

                dataDumper.PrintDisclaimer();
                var result = (await context.Resolve<IRealEstateAgentsBl>().GetTopSellers(opts.SearchTerms, opts.NumberOfResults));

                dataDumper.DumpToConsole(result, opts.NumberOfResults, opts.UseFakeApi);
            }
        }

        private static void RegisterServiceModules(GetRealEstateAgentSalesCommandOptions opts, ContainerBuilder builder)
        {
            if (opts.UseFakeApi)
                builder.RegisterModule<ConsoleFakeServicesModules>();
            else
            {
                builder.RegisterModule<ConsoleRealServicesModules>();
                if (opts.UseSingleThreadedApiIntegration)
                    builder.RegisterModule<SimpleApiIntegrationServicesModules>();
                else
                    builder.RegisterModule<ParallelApiIntegrationServicesModules>();
            }

        }
    }
}
