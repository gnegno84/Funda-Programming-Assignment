using System.Collections.Generic;
using Funda.ProgrammingAssignment.Domain.Common.Dto;

namespace Funda.ProgrammingAssignment.Console.ConsoleDumpers.TableDumper
{
    public interface IRealEstatesAgentSalesDataConsoleDumper
    {
        void DumpToConsole(IEnumerable<RealEstateAgentSalesDto> agentSales, IEnumerable<string> searchTerms, int numberOfRequestedResults, bool isFakeApiResult);
    }
}