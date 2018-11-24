using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funda.ProgrammingAssignment.Domain.Common.Dto;
using Funda.ProgrammingAssignment.Domain.Common.Services;
using Funda.ProgrammingAssignment.ServiceProxy.Model.ApiResults;

namespace Funda.ProgrammingAssignment.ServiceProxy
{
    public class FundaApiBasedRepository : IPropertiesRepository
    {
        private readonly IFundaProxyApiService _fundaProxyApiService;

        public FundaApiBasedRepository(IFundaProxyApiService fundaProxyApiService)
        {
            _fundaProxyApiService = fundaProxyApiService;
        }

        public async Task<IEnumerable<PropertyDto>> SearchPropertiesOnSale(IEnumerable<string> searchTerms)
        {
            var defaultPageSize = 25;
            var maxRetrievablePages = 4000;
            var currentPage = 0;

            PagedApiResult<PropertyDto> pagedResult;
            List<PropertyDto> res = new List<PropertyDto>();

            do
            {
                pagedResult = await _fundaProxyApiService.GetPropertiesOnSale(searchTerms, currentPage, defaultPageSize);
                currentPage += 1;
                if (pagedResult.WasSuccessfull)
                {
                    if (pagedResult.NumberOfPages >= maxRetrievablePages)
                    {
                        //TODO: Handle Exception too many pages
                    }

                    res.AddRange(pagedResult.Data);
                }
                else
                {
                    //TODO: In this case we can inject different strategies to handle the different errors. E.g.: In case of 429 response (Too Many Requests) we can choose to slow down the requests and retry or return an information to the user to inform that he should try again later. Right now I'll simply return an Exception

                    throw new Exception(pagedResult.FailureReason);
                }

            } while (pagedResult.HasNextPageInPagination);

            return res;
        }
    }
}