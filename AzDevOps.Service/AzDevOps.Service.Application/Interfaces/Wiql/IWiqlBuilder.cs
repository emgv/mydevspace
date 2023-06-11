using Shared.Enums;

namespace AzDevOps.Service.Application.Interfaces.Wiql;

public interface IWiqlBuilder
{
    void Reset();
    IWiqlBuilder AddPagingByIds(SortEnum sortType, int currentPage, int nextPage, long firstId, long lastId);
    IWiqlBuilder AddFilterOnlyByMyWorkItems();
    string Build();
}
