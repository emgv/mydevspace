using Shared.Enums;
using System.Net;
using System.Web;

namespace Shared.Dtos.AzDevOps.WorkItems;

public class AzWorkItemsSearchParametersDto
{
    public string? SearchText { get; set; }
    public string SortColumnName { get => "id"; }
    public SortEnum SortType { get; set; } = SortEnum.Desc;
    public int CurrentPage { get; set; }
    public int NextPage { get; set; }
    public int PageSize { get => 5; }
    public long? FirstId { get; set; }
    public long? LastId { get; set; }

    public string ToHttpQueryString()
    {
        string query = $"?SortType={HttpUtility.UrlEncode(SortType.ToString())}";

        if(!string.IsNullOrEmpty(SearchText))
            query += $"&SearchText={HttpUtility.UrlEncode(SearchText)}";

        if(FirstId != null)
            query += $"&FirstId={FirstId}";

        if (LastId != null)
            query += $"&LastId={LastId}";

        query += $"&SortColumnName={HttpUtility.UrlEncode(SortColumnName)}";
        query += $"&PageSize={PageSize}";
        query += $"&CurrentPage={CurrentPage}";
        query += $"&NextPage={NextPage}";
        
        return query;
    }
}
