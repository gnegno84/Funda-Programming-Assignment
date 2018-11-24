using System.Collections.Generic;
using Funda.ProgrammingAssignment.ServiceProxy.Model;
using Funda.ProgrammingAssignment.ServiceProxy.Services.ApiConfigurationParser;
using RestSharp;

namespace Funda.ProgrammingAssignment.ServiceProxy.Services.RequestBuilder
{
    public class RestRequestsBuilder : IRestRequestsBuilder
    {

        private readonly IApiConfigurationParser _apiConfigurationParser;

        public RestRequestsBuilder(IApiConfigurationParser apiConfigurationParser)
        {
            _apiConfigurationParser = apiConfigurationParser;
        }

        public IRestClient BuildDefaultClient()
        {
           return new RestClient(_apiConfigurationParser.GetBasicConfiguration().BaseUrl);
        }

        public IRestRequest BuildGet(List<Parameter> queryStringParameters)
        {
            if(queryStringParameters == null)
                queryStringParameters = new List<Parameter>();

            var apiConfig = _apiConfigurationParser.GetBasicConfiguration();

            var request = new RestRequest(Method.GET)
            {
                Resource = BuildResourcePathWithKey(apiConfig)
            };

            foreach (var optionalParameter in queryStringParameters)
            {
                request.AddOrUpdateParameter(optionalParameter);
            }

            return request;
        }

        private static string BuildResourcePathWithKey(BasicApiConfiguration apiConfig)
        {
            return $"{apiConfig.BaseUrl}/json/{apiConfig.Key}/";
        }
    }
}