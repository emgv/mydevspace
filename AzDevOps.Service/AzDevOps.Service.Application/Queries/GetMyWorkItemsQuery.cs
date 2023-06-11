using MediatR;
using Shared.Dtos;
using Shared.Dtos.AzDevOps.WorkItems;

namespace AzDevOps.Service.Application.Queries;

public record GetMyWorkItemsQuery(AzWorkItemsSearchParametersDto SearchParameteres)
    : IRequest<PagedResponseDto<WorkItemDto>>;

