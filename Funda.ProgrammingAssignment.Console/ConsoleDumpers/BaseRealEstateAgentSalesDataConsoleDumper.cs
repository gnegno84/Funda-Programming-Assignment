﻿using System;
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

        public void DumpToConsole(IEnumerable<RealEstateAgentSalesDto> agentSales, IEnumerable<string> searchTerms, int numberOfRequestedResults, bool isFakeApiResult)
        {
            if (!agentSales.Any())
                System.Console.WriteLine("No available results found for selected criteria.");
            else
            {
                DumpHeader(searchTerms, numberOfRequestedResults, isFakeApiResult);
                DumpToConsoleUsingSpecificDumper(agentSales);
            }
        }

        private static void DumpHeader(IEnumerable<string> searchTerms, int numberOfRequestedResults, bool isFakeApiResult)
        {
            Colorful.Console.WriteLine();
            Colorful.Console.WriteAscii("Funda", Color.DarkGoldenrod);
            Colorful.Console.WriteLine();
            Colorful.Console.Write($"Showing first {numberOfRequestedResults} results for the search of: ");
            Colorful.Console.Write($"{string.Join(" - ", searchTerms)}", Color.DarkGoldenrod);
            Colorful.Console.WriteLine();
            if (isFakeApiResult)
            {
                Colorful.Console.WriteLine();
                Colorful.Console.WriteLine("------------ THIS IS A FAKE RESULT GENERATED USING FAKE INTEGRATION API ------------", Color.Crimson);
            }

            Colorful.Console.ResetColor();
        }
    }
}