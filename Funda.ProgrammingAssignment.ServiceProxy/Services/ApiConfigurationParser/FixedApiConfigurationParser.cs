using System;
using Funda.ProgrammingAssignment.ServiceProxy.Model;
using Microsoft.Extensions.Configuration;

namespace Funda.ProgrammingAssignment.ServiceProxy.Services.ApiConfigurationParser
{
    public class FixedApiConfigurationParser : IApiConfigurationParser
    {
        public const string DefaultBaseUrl = "http://partnerapi.funda.nl/feeds/Aanbod.svc/";
        public const string DefaultKey = "ac1b0b1572524640a0ecc54de453ea9f";
        public const int DefaultNumberOfConcurrentApiCalls = 16;

        public BasicApiConfiguration GetBasicConfiguration()
        {
            return new BasicApiConfiguration
            {
                BaseUrl = DefaultBaseUrl,
                Key = DefaultKey,
                ConcurrentRequestsNumber = DefaultNumberOfConcurrentApiCalls
            };
        }

    }

    public class FromAppSettingsApiConfigurationParser : IApiConfigurationParser
    {
        private readonly IConfiguration _config;

        public FromAppSettingsApiConfigurationParser(IConfiguration config)
        {
            _config = config;
        }

        public BasicApiConfiguration GetBasicConfiguration()
        {
            var confSection = _config.GetSection("ApiSettings");

            var baseUrl = confSection["BaseUrl"];
            if(string.IsNullOrWhiteSpace(baseUrl))
                throw new Exception("Missing BaseUrl conf");

            var key = confSection["Key"];
            if (string.IsNullOrWhiteSpace(key))
                throw new Exception("Missing Key conf");

            var numParallelReq = confSection["NumberOfParallelRequests"];
            if (string.IsNullOrWhiteSpace(key))
                throw new Exception("Missing NumberOfParallelRequests conf");

            return new BasicApiConfiguration
            {
                BaseUrl = baseUrl,
                Key = key,
                ConcurrentRequestsNumber = int.Parse(numParallelReq)
            };
        }

    }
}