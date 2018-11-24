using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Funda.ProgrammingAssignment.Console.ConsoleDumpers.TableDumper;
using Funda.ProgrammingAssignment.Console.Modules;
using Funda.ProgrammingAssignment.Domain.BL;

namespace Funda.ProgrammingAssignment.Console.Commands.GetRealEstateAgentSales
{
    class GetRealEstateAgentSalesCommandHandler
    {
        public static int RunGetRealEstateAgentSales(GetRealEstateAgentSalesCommandOptions opts)
        {
            Task.Run(() => GetTopRealEstateAgentSalesAsync(opts)).Wait();
            return 0;
        }

        private static async Task GetTopRealEstateAgentSalesAsync(GetRealEstateAgentSalesCommandOptions opts)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<ConsoleBaseModules>();
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
                builder.RegisterModule<ConsoleRealServicesModules>();
        }
    }
}
