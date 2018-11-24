using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funda.ProgrammingAssignment.Domain.Common.Dto;
using Funda.ProgrammingAssignment.Domain.Common.Services;
using Funda.ProgrammingAssignment.Domain.Services.ResultMapper;

namespace Funda.ProgrammingAssignment.Domain.BL
{
    public class RealEstateAgentsBl : IRealEstateAgentsBl
    {
        private readonly IPropertiesRepository _apiProxyService;
        private readonly IResultMapper _resultMapper;

        public RealEstateAgentsBl(IPropertiesRepository apiProxyService, IResultMapper resultMapper)
        {
            _apiProxyService = apiProxyService;
            _resultMapper = resultMapper;
        }

        public async Task<IEnumerable<RealEstateAgentSalesDto>> GetTopSellers(IEnumerable<string> searchTerms, int howMany)
        {
            if (howMany <= 0) throw new ArgumentOutOfRangeException(nameof(howMany));
            if (searchTerms == null || !searchTerms.Any())
                throw new ArgumentException("Value cannot be an empty collection.", nameof(searchTerms));

            var res = await _apiProxyService.SearchPropertiesOnSale(searchTerms);

            return _resultMapper.MapToAgentSales(res).OrderByDescending(rea => rea.NumberOfPropertiesInSale);
        }
    }
}

