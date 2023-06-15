using BFF.Api.Configurations;
using BFF.Api.HttpClients;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using Shared.Dtos.AzDevOps.WorkItems;

namespace BFF.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkItemsController : ControllerBase
    {
        private readonly HttpClient _workItemsClient;

        public WorkItemsController(WorkItemsClientFactory workItemsClientFactory)
        {
            _workItemsClient = workItemsClientFactory.CreateClient(nameof(ServicesAPIsConfig.AzDevOpsServiceAPI));
        }

        [HttpGet("get-my-work-items")]
        public async Task<PagedResponseDto<WorkItemDto>> GetMyWorkItems(
            [FromQuery] AzWorkItemsSearchParametersDto searchParameteres, CancellationToken cancellationToken = default)
        {
            string uri = $"WorkItems/get-my-work-items{searchParameteres.ToHttpQueryString()}";
            return await _workItemsClient.GetFromJsonAsync<PagedResponseDto<WorkItemDto>>(uri, cancellationToken)
                ?? throw new InvalidOperationException("Could not get the list of work items");
        }
    }
}
