using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
            var defaultConcurrentRequestsNumber = 10;

            ConcurrentQueue<PropertyDto> res = new ConcurrentQueue<PropertyDto>();

            var pagedResult = await ExecuteApiCall(searchTerms, 0, defaultPageSize, res);
            if (pagedResult.NumberOfPages >= maxRetrievablePages)
            {
                //TODO: Handle Exception too many pages. This is a circuit breaker in case of huge amount of data to request. It could be configred with higher values or removed as requested and handled in a different way.
                throw new Exception("The request contains too many entries to be processed!");
            }

            var numberOfRequestsToPerform = pagedResult.NumberOfPages % defaultConcurrentRequestsNumber == 0
                ? pagedResult.NumberOfPages / defaultConcurrentRequestsNumber
                : (pagedResult.NumberOfPages / defaultConcurrentRequestsNumber) + 1;

            for (var i = 1; i < numberOfRequestsToPerform; i++)
            {
                ProceedInBatch(defaultConcurrentRequestsNumber, i, searchTerms, defaultPageSize, res);
            }

            return res;
        }

        private async Task<PagedApiResult<PropertyDto>> ExecuteApiCall(IEnumerable<string> searchTerms, int currentPage, int pageSize, ConcurrentQueue<PropertyDto> res)
        {
            var pagedResult = await _fundaProxyApiService.GetPropertiesOnSale(searchTerms, currentPage, pageSize);
            if (pagedResult.WasSuccessfull)
            {
                foreach (var data in pagedResult.Data)
                    res.Enqueue(data);
            }
            else
            {
                //TODO: In this case we can inject different strategies to handle the different errors. E.g.: In case of 429 response (Too Many Requests) we can choose to slow down the requests and retry or return an information to the user to inform that he should try again later. Right now I'll simply return an Exception
                throw new Exception(pagedResult.FailureReason);
            }

            return pagedResult;
        }

        public void ProceedInBatch(int batchSize, int currentPageIndex, IEnumerable<string> searchTerms, int pageSize, ConcurrentQueue<PropertyDto> res)
        {

            List<Task> tasks = new List<Task>();
            foreach (var pageIndex in Enumerable.Range(currentPageIndex, batchSize))
            {
                tasks.Add(ExecuteApiCall(searchTerms, pageIndex, pageSize, res));
            }
            Task.WaitAll(tasks.ToArray());
        }
    }
}