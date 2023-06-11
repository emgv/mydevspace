using Shared.Enums;

namespace Shared.Dtos;

public class PagedResponseDto<TResponseType>
{
    public IEnumerable<TResponseType> Data { get; set; } = Enumerable.Empty<TResponseType>();
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public (long FirstId, long LastId)? Interval { get; set; }
    public string SortColumnName { get; set; }
    public SortEnum SortType { get; set; }
}
