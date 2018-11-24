using System.Collections.Generic;
using System.Data;
using Funda.ProgrammingAssignment.Domain.Common.Dto;

namespace Funda.ProgrammingAssignment.Console.ConsoleDumpers.TableDumper
{
    public class RealEstateAgentSalesDataTableCreator : IRealEstateAgentSalesDataTableCreator
    {
        public DataTable CreateDataTable(IEnumerable<RealEstateAgentSalesDto> agentSales)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("N. Of Objects", typeof(int));
            table.Columns.Add("Cities where selling", typeof(string));

            foreach (var agentSale in agentSales)
            {
                table.Rows.Add(agentSale.AgentId, agentSale.AgentName, agentSale.NumberOfPropertiesInSale, string.Join(" - ", agentSale.CitiesWhereIsSelling));
            }

            return table;
        }
    }
}