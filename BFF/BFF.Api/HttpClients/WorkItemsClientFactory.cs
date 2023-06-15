using BFF.Api.Configurations;
using Microsoft.Extensions.Options;

namespace BFF.Api.HttpClients
{
    public class WorkItemsClientFactory : IHttpClientFactory
    {
        private readonly ServicesAPIsConfig _servicesAPIsConfig;
        private readonly IDictionary<string, string> _servicesAPIs;

        public WorkItemsClientFactory(IOptions<ServicesAPIsConfig> servicesAPIsConfig)
        {
            _servicesAPIsConfig = servicesAPIsConfig.Value;
            _servicesAPIs = new Dictionary<string, string>()
            {
                { nameof(ServicesAPIsConfig.AzDevOpsServiceAPI).ToLower(), _servicesAPIsConfig.AzDevOpsServiceAPI }
            };
        }

        public HttpClient CreateClient(string name)
        {
            if(string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            bool success = _servicesAPIs.TryGetValue(name.ToLower(), out string? serviceBaseAddress);
            if (!success)
                throw new ArgumentException($"Could not find the http-client service with name {name}. Please check the config {nameof(ServicesAPIsConfig)}.");

            if (string.IsNullOrEmpty(serviceBaseAddress))
                throw new InvalidDataException($"Incorrect service base address given, please check the config {nameof(ServicesAPIsConfig)}.");

            return new HttpClient()
            {
                BaseAddress = new Uri(serviceBaseAddress)
            };
        }
    }
}
