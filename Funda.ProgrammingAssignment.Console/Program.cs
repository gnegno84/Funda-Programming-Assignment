using System;
using System.IO;
using CommandLine;
using Funda.ProgrammingAssignment.Console.Commands.GetRealEstateAgentSales;
using Microsoft.Extensions.Configuration;

namespace Funda.ProgrammingAssignment.Console
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                IConfigurationRoot configuration = builder.Build();

                var res = Parser.Default.ParseArguments<GetRealEstateAgentSalesCommandOptions>(args)
                    .MapResult(options => GetRealEstateAgentSalesCommandHandler.RunGetRealEstateAgentSales(options, configuration), errs => 1);

                System.Console.ReadKey();
                return res;
            }
            catch (Exception ex)
            {
                System.Console.Error.WriteLine(ex);
                System.Console.ReadKey();

                return -1;
            }
        }
    }
}
