using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funda.ProgrammingAssignment.Domain.Common.Dto;
using Funda.ProgrammingAssignment.Domain.Common.Services;
using Funda.ProgrammingAssignment.ServiceProxy.Model.ApiResults;
using Funda.ProgrammingAssignment.ServiceProxy.Services.ApiConfigurationParser;
using Funda.ProgrammingAssignment.ServiceProxy.Services.RequestStatusUpdater;
using Microsoft.Extensions.Logging;


namespace Funda.ProgrammingAssignment.ServiceProxy
{
    public class ParallellizedFundaApiBasedRepository : IPropertiesRepository
    {
        public const int DefaultPageSize = 25;
        public const int MaxRetrievablePages = 4000;
        public const int DefaultConcurrentRequestsNumber = 10;

        private readonly IFundaProxyApiService _fundaProxyApiService;
        private readonly ILogger _logger;
        private readonly IRequestStatusUpdater _requestStatusUpdater;
        private readonly int _concurrentRequestsNumber;

        public ParallellizedFundaApiBasedRepository(IFundaProxyApiService fundaProxyApiService,
            IApiConfigurationParser configurationParser, ILogger logger, IRequestStatusUpdater requestStatusUpdater)
        {
            _fundaProxyApiService = fundaProxyApiService;
            _logger = logger;
            _requestStatusUpdater = requestStatusUpdater;

            _concurrentRequestsNumber = configurationParser.GetBasicConfiguration().ConcurrentRequestsNumber == 0
                ? DefaultConcurrentRequestsNumber
                : configurationParser.GetBasicConfiguration().ConcurrentRequestsNumber;
        }

        public async Task<IEnumerable<PropertyDto>> SearchPropertiesOnSale(IEnumerable<string> searchTerms)
        {
            ConcurrentQueue<PropertyDto> res = new ConcurrentQueue<PropertyDto>();

            //I have to start from page 1 as the page 0 and one have the same data replicated
            var pagedResult = await ExecuteApiCall(searchTerms, 1, DefaultPageSize, res);
            _logger.LogTrace($"Number of pages to inquire: {pagedResult.NumberOfPages} - Will be fetched using {_concurrentRequestsNumber} requests in parallel");

            ReturnExceptionIfRequestedTooManyPagesToRetrieve(pagedResult, MaxRetrievablePages);

            if (pagedResult.HasNextPageInPagination)
                RetrieveAllPagesData(searchTerms, pagedResult, res);

            return res;
        }

        private static void ReturnExceptionIfRequestedTooManyPagesToRetrieve(PagedApiResult<PropertyDto> pagedResult,
            int maxRetrievablePages)
        {
            if (pagedResult.NumberOfPages >= maxRetrievablePages)
            {
                throw new Exception("The request contains too many entries to be processed!");
            }
        }

        private void RetrieveAllPagesData(IEnumerable<string> searchTerms, PagedApiResult<PropertyDto> pagedResult,
            ConcurrentQueue<PropertyDto> res)
        {
            var numberOfRequestsToPerform = CalculateNumberOfBatchRequestsToPerform(pagedResult, _concurrentRequestsNumber);

            _requestStatusUpdater.Initialize(numberOfRequestsToPerform, "Retrieving properties data...");

            ExecuteRequestBatches(searchTerms, numberOfRequestsToPerform, pagedResult.NumberOfPages, res);
        }

        private static int CalculateNumberOfBatchRequestsToPerform(PagedApiResult<PropertyDto> pagedResult,
            int defaultConcurrentRequestsNumber)
        {
            var remainingNumberOfPagesToRetrieve = pagedResult.NumberOfPages - 1;
            var numberOfRequestsToPerform = remainingNumberOfPagesToRetrieve % defaultConcurrentRequestsNumber == 0
                ? remainingNumberOfPagesToRetrieve / defaultConcurrentRequestsNumber
                : (remainingNumberOfPagesToRetrieve / defaultConcurrentRequestsNumber) + 1;
            return numberOfRequestsToPerform;
        }

        private void ExecuteRequestBatches(IEnumerable<string> searchTerms, int numberOfRequestsToPerform,
            int totalNumberOfPages, ConcurrentQueue<PropertyDto> res)
        {
            for (var i = 0; i < numberOfRequestsToPerform; i++)
            {
                ProceedInBatch(_concurrentRequestsNumber, i, totalNumberOfPages, searchTerms, DefaultPageSize, res);
                _requestStatusUpdater.Tick();
            }
        }

        private void ProceedInBatch(int batchSize, int currentBatch, int totalNumberOfPages,
            IEnumerable<string> searchTerms, int apiRequestPageSize, ConcurrentQueue<PropertyDto> res)
        {
            var startingPageNumber = currentBatch * batchSize + 2;
            var requestsInBatch = Math.Min(batchSize, (totalNumberOfPages - startingPageNumber)+1);
            Task.WaitAll(
                Enumerable.Range(startingPageNumber, requestsInBatch)
                    .Select(pageIndex =>
                    {
                        _logger.LogTrace($"--- Request for page {pageIndex}");
                        return ExecuteApiCall(searchTerms, pageIndex, apiRequestPageSize, res);
                    })
                    .Cast<Task>()
                    .ToArray()
            );
        }

        private async Task<PagedApiResult<PropertyDto>> ExecuteApiCall(IEnumerable<string> searchTerms, int currentPage,
            int pageSize, ConcurrentQueue<PropertyDto> res)
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

      
    }
}