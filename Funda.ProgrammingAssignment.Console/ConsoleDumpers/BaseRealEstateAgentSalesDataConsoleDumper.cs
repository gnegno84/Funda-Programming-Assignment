using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Funda.ProgrammingAssignment.Console.ConsoleDumpers.TableDumper;
using Funda.ProgrammingAssignment.Domain.Common.Dto;

namespace Funda.ProgrammingAssignment.Console.ConsoleDumpers
{
    public abstract class BaseRealEstateAgentSalesDataConsoleDumper : IRealEstatesAgentSalesDataConsoleDumper
    {
        protected abstract void DumpToConsoleUsingSpecificDumper(IEnumerable<RealEstateAgentSalesDto> agentSales);

        public void DumpToConsole(GetTopSellersResult result, int numberOfRequestedResults, bool isFakeApiResult)
        {
            if (result == null || !result.AgentSales.Any())
                System.Console.WriteLine("No available results found for selected criteria.");
            else
            {
                DumpHeader(result, numberOfRequestedResults, isFakeApiResult);
                DumpToConsoleUsingSpecificDumper(result.AgentSales);
            }
        }

        public void PrintDisclaimer()
        {
            Colorful.Console.WriteLine();
            Colorful.Console.WriteAscii("Funda", Color.DarkGoldenrod);
            Colorful.Console.WriteLine();

            Colorful.Console.WriteLine("Trying to fetch all the required data for the request. It could take some time...");
        }

        private static void DumpHeader(GetTopSellersResult result, int numberOfRequestedResults, bool isFakeApiResult)
        {
            Colorful.Console.Write($"Showing first {numberOfRequestedResults} results for the search of: ");
            Colorful.Console.Write($"{string.Join(" - ", result.SearchTerms)}", Color.DarkGoldenrod);
            Colorful.Console.WriteLine();
            if (isFakeApiResult)
            {
                Colorful.Console.WriteLine();
                Colorful.Console.WriteLine("------------ THIS IS A FAKE RESULT GENERATED USING FAKE INTEGRATION API ------------", Color.Crimson);
            }

            Colorful.Console.ResetColor();
            Colorful.Console.WriteLine($"Total Number of processed objects: {result.TotalNumberOfObjectsForRequest}", Color.CornflowerBlue);
            Colorful.Console.WriteLine($"Elapsed Time: {result.ElapsedTime}", Color.CornflowerBlue);
        }
    }
}