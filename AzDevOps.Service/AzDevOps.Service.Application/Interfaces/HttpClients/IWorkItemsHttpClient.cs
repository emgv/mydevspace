using Shared.Dtos;
using Shared.Dtos.AzDevOps.WorkItems;

namespace AzDevOps.Service.Application.Interfaces.HttpClients;

public interface IWorkItemsHttpClient
{
    Task<PagedResponseDto<WorkItemDto>> GetMyWorkItems(
        AzWorkItemsSearchParametersDto search, CancellationToken cancellationToken);
}
