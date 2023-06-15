using AzDevOps.Service.Application.Configurations;
using AzDevOps.Service.Application.Interfaces.HttpClients;
using AzDevOps.Service.Application.Interfaces.Wiql;
using Microsoft.Extensions.Options;
using Shared.Dtos;
using Shared.Dtos.AzDevOps.WorkItems;
using System.Net.Http.Json;
using System.Text.Json;

namespace AzDevOps.Service.Infrastructure.HttpClients;

public class WorkItemsHttpClient : IWorkItemsHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IWiqlBuilder _wiqlBuilder;
    private readonly AzDevOpsConfig _azConfig;

    public WorkItemsHttpClient(
        HttpClient httpClient,
        IWiqlBuilder wiqlBuilder,
        IOptions<AzDevOpsConfig> config)
    {
        _httpClient = httpClient;
        _wiqlBuilder = wiqlBuilder;
        _azConfig = config.Value;
    }

    public async Task<PagedResponseDto<WorkItemDto>> GetMyWorkItems(
        AzWorkItemsSearchParametersDto search, CancellationToken cancellationToken)
    {
        var uri = $"wit/wiql?$top={search.PageSize}&api-version={_azConfig.ApiVersion}";
        var query = new WiqlDto()
        {
            Query = _wiqlBuilder
                    .AddFilterOnlyByMyWorkItems()
                    .AddPagingByIds(
                        search.SortType, search.CurrentPage, search.NextPage,
                        search.FirstId ?? 0,
                        search.LastId ?? 0)
                    .Build()
        };
        var response = await _httpClient.PostAsJsonAsync<WiqlDto>(uri, query, cancellationToken);

        response.EnsureSuccessStatusCode();
        var queryResponse = await response.Content.ReadFromJsonAsync<WiqlResponseDto>(
            (JsonSerializerOptions?)null, cancellationToken);

        if (queryResponse == null)
            throw new InvalidDataException("Could not get the WIQL response");

        if (queryResponse.WorkItems == null || !queryResponse.WorkItems.Any())
            return new PagedResponseDto<WorkItemDto>()
            {
                PageIndex = search.NextPage,
                PageSize = search.PageSize,
                SortColumnName = search.SortColumnName,
                SortType = search.SortType
            };

        var workItems = new List<WorkItemDto>(queryResponse.WorkItems.Count());
        foreach(var wit in queryResponse.WorkItems)
        {
            var workItemData = await GetWorkItemById(wit.Id, cancellationToken);
            if(workItemData == null)
                throw new InvalidDataException($"Could not get the work-item with id {wit.Id}");
            workItems.Add(workItemData);
        }

        return new PagedResponseDto<WorkItemDto>()
        {
            Data = workItems,
            PageIndex = search.NextPage,
            PageSize = search.PageSize,
            SortColumnName = search.SortColumnName,
            FirstId = workItems.First().Id,
            LastId = workItems.Last().Id,
            SortType = search.SortType
        };
    }

    public async Task<WorkItemDto?> GetWorkItemById(long workItemId, CancellationToken cancellationToken)
    {
        var uri = $"wit/workitems/{workItemId}?api-version={_azConfig.ApiVersion}";
        return await _httpClient.GetFromJsonAsync<WorkItemDto>(uri, cancellationToken);
    }
}