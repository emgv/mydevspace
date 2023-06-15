using AzDevOps.Service.Application.Configurations;
using AzDevOps.Service.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Shared.Dtos;
using Shared.Dtos.AzDevOps.WorkItems;
using System.Net;
using System.Net.Http.Headers;

namespace AzDevOps.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkItemsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly AzDevOpsConfig _azDevOpsConfig;

        public WorkItemsController(
            IMediator mediator,
            IOptions<AzDevOpsConfig> azDevOpsConfig)
        {
            _mediator = mediator;
            _azDevOpsConfig = azDevOpsConfig.Value;
        }

        [HttpGet("get-my-work-items")]
        public async Task<PagedResponseDto<WorkItemDto>> GetMyWorkItems([FromQuery] AzWorkItemsSearchParametersDto searchParameteres)
        {
            // TODO: check work-items batch request, because it can receive the wit-ids and also the __list of response-fields__
            return await _mediator.Send(new GetMyWorkItemsQuery(searchParameteres));
        }

        [HttpGet("sample-http-client")]
        public async Task<WorkItemDto?> Sample()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                       new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(
                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                string.Format("{0}:{1}", "", _azDevOpsConfig.PAT))));

                    var workItem = await client.GetFromJsonAsync<WorkItemDto>(
                        $"https://dev.azure.com/{_azDevOpsConfig.Organization}/{_azDevOpsConfig.Project}/_apis/wit/workitems/123456?api-version=7.0");

                    return workItem;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }

        [HttpGet("sample-vss")]
        public async Task<WorkItem> SampleVSS()
        {
            VssBasicToken token = new VssBasicToken(new NetworkCredential("", _azDevOpsConfig.PAT));
            VssCredentials credentials = new VssBasicCredential(token);
            VssConnection connection = new VssConnection(new Uri($"https://dev.azure.com/{_azDevOpsConfig.Organization}"), credentials);

            var witClient = connection.GetClient<WorkItemTrackingHttpClient>();
            var wit = await witClient.GetWorkItemAsync(_azDevOpsConfig.Project, id: 123456);
            return wit;
        }
    }
}
