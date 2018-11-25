using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funda.ProgrammingAssignment.Domain.Common.Dto;
using Funda.ProgrammingAssignment.Domain.Common.Services;
using Funda.ProgrammingAssignment.ServiceProxy.Model.ApiResults;
using Funda.ProgrammingAssignment.ServiceProxy.Services.ProxyService;
using Microsoft.Extensions.Logging;

namespace Funda.ProgrammingAssignment.ServiceProxy
{
    //The simplest way to get the data. This Repository will use the ApiService in order to get all the pages (do GET while HASNEXTPAGEINPAGINATION) of the result.
    //Not all code has been implemented (special case handling like too many pages or failure in retrieving the result of a page). A recovery strategy injection approach is what i would suggest in this scenario
    public class FundaApiBasedRepository : IPropertiesRepository
    {
        private readonly IFundaProxyApiService _fundaProxyApiService;
        private readonly ILogger _logger;

        public FundaApiBasedRepository(IFundaProxyApiService fundaProxyApiService,ILogger logger)
        {
            _fundaProxyApiService = fundaProxyApiService;
            _logger = logger;
        }

        public async Task<IEnumerable<PropertyDto>> SearchPropertiesOnSale(IEnumerable<string> searchTerms)
        {
            var defaultPageSize = 25;
            var maxRetrievablePages = 4000;
            var currentPage = 1; //Starts from page 1 as the page 0 and 1 have the same data duplicated =/

            PagedApiResult<PropertyDto> pagedResult;
            List<PropertyDto> res = new List<PropertyDto>();

            do
            {
                _logger.LogTrace($"---- Retrieving page {currentPage} (size:{defaultPageSize})");
                pagedResult = await _fundaProxyApiService.GetPropertiesOnSale(searchTerms, currentPage, defaultPageSize);
                currentPage += 1;
                if (pagedResult.WasSuccessfull)
                {
                    if (pagedResult.NumberOfPages >= maxRetrievablePages)
                    {
                        throw new Exception("The result's page number is to big to be handled. Sorry");
                        //TODO: Handle Exception too many pages in a more useful way =)
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