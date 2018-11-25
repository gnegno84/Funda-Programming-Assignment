using System;
using Funda.ProgrammingAssignment.Domain.Common.Exceptions;
using Funda.ProgrammingAssignment.ServiceProxy.Model;
using Microsoft.Extensions.Configuration;

namespace Funda.ProgrammingAssignment.ServiceProxy.Services.ApiConfigurationParser
{
    public class FromAppSettingsApiConfigurationParser : IApiConfigurationParser
    {
        protected internal const string ConfigSectionName = "ApiSettings";
        protected internal const string BaseUrlConfigName = "BaseUrl";
        protected internal const string KeyConfigName = "Key";
        protected internal const string NumberOfParallelRequestsConfigName = "NumberOfParallelRequests";
        private readonly IConfiguration _config;

        public FromAppSettingsApiConfigurationParser(IConfiguration config)
        {
            _config = config;
        }

        public BasicApiConfiguration GetBasicConfiguration()
        {
            var confSection = _config.GetSection(ConfigSectionName);

            if (confSection == null)
                throw new ConfigurationException(ConfigSectionName, "Cannot find the specified section in config file");

            var baseUrl = confSection[BaseUrlConfigName];
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ConfigurationException(BaseUrlConfigName, "Missing BaseUrl conf");

            var key = confSection[KeyConfigName];
            if (string.IsNullOrWhiteSpace(key))
                throw new ConfigurationException(KeyConfigName, "Missing Key conf");

            var numParallelReq = confSection[NumberOfParallelRequestsConfigName];
            if (string.IsNullOrWhiteSpace(key))
                throw new ConfigurationException(NumberOfParallelRequestsConfigName, "Missing NumberOfParallelRequests conf");
            if(!int.TryParse(numParallelReq, out var parallelRequests) || parallelRequests <= 0) 
                throw new ConfigurationException(NumberOfParallelRequestsConfigName, $"The {NumberOfParallelRequestsConfigName} parameter must be a valid integer greater than zero.");

            return new BasicApiConfiguration
            {
                BaseUrl = baseUrl,
                Key = key,
                ConcurrentRequestsNumber = parallelRequests
            };
        }

    }
}