using System;
using CommandLine;
using Funda.ProgrammingAssignment.Console.Commands.GetRealEstateAgentSales;

namespace Funda.ProgrammingAssignment.Console
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                var res = Parser.Default.ParseArguments<GetRealEstateAgentSalesCommandOptions>(args)
                    .MapResult(GetRealEstateAgentSalesCommandHandler.RunGetRealEstateAgentSales, errs => 1);

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
