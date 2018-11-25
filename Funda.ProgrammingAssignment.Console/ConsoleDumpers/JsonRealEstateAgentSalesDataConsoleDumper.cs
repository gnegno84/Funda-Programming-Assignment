using System.Collections.Generic;
using Funda.ProgrammingAssignment.Domain.Common.Dto;
using Newtonsoft.Json;

namespace Funda.ProgrammingAssignment.Console.ConsoleDumpers
{
    //This "Dumper" will write to console the json serialization of the results
    public class JsonRealEstateAgentSalesDataConsoleDumper : BaseRealEstateAgentSalesDataConsoleDumper
    {
        protected override void DumpToConsoleUsingSpecificDumper(IEnumerable<RealEstateAgentSalesDto> agentSales)
        {
            var serializedResult = JsonConvert.SerializeObject(agentSales, Formatting.Indented);
            System.Console.WriteLine(serializedResult);
        }
    }
}