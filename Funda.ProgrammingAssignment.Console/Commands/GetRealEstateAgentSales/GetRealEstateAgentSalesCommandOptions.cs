using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;
using Funda.ProgrammingAssignment.Console.Model;

namespace Funda.ProgrammingAssignment.Console.Commands.GetRealEstateAgentSales
{
    [Verb("topSellingAgents", HelpText = "Print a list of the top N real estate agents that have the most number of properties listed for sale. You can define the search terms and the number of results")]
    internal class GetRealEstateAgentSalesCommandOptions
    {
        [Option("fa", HelpText = "Set as true to use fake api integrations instead of the real one (testing purposes only)", Default = false, Required = false)]
        public bool UseFakeApi { get; set; }

        [Option("sth", HelpText = "Set to use a slower, single threaded execution of the api request.", Default = false, Required = false)]
        public bool UseSingleThreadedApiIntegration { get; set; }

        [Option("rn", HelpText = "Select the number of results.", Default = 10, Required = false)]
        public int NumberOfResults { get; set; }

        [Option('t', HelpText = "The search terms, separated by a space", Required = true)]
        public IEnumerable<string> SearchTerms { get; set; }

        [Option('o', HelpText = "Select the type of console output (Table/Json).", Required = false, Default = DumperTypeEnum.Table)]
        public DumperTypeEnum OutputType { get; set; }
    }
}
