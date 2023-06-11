using Shared.Enums;

namespace Shared.Dtos.AzDevOps.WorkItems;

public class AzWorkItemsSearchParametersDto
{
    public string? SearchText { get; set; }
    public string SortColumnName { get => "id"; }
    public SortEnum SortType { get; set; } = SortEnum.Desc;
    public int CurrentPage { get; set; }
    public int NextPage { get; set; }
    public int PageSize { get => 5; }
    public (long FirstId, long LastId)? Interval { get; set; }
}
