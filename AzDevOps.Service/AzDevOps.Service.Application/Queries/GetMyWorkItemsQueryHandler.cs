using AzDevOps.Service.Application.Interfaces.HttpClients;
using MediatR;
using Shared.Dtos;
using Shared.Dtos.AzDevOps.WorkItems;

namespace AzDevOps.Service.Application.Queries;

public class GetMyWorkItemsQueryHandler : IRequestHandler<GetMyWorkItemsQuery, PagedResponseDto<WorkItemDto>>
{
    private readonly IWorkItemsHttpClient _workItemsHttpClient;

    public GetMyWorkItemsQueryHandler(IWorkItemsHttpClient workItemsHttpClient)
    {
        _workItemsHttpClient = workItemsHttpClient;
    }

    public Task<PagedResponseDto<WorkItemDto>> Handle(GetMyWorkItemsQuery req, CancellationToken cancellationToken)
    {
        return _workItemsHttpClient.GetMyWorkItems(req.SearchParameteres, cancellationToken);
    }
}
