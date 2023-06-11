using AzDevOps.Service.Application.Interfaces.Wiql;
using Shared.Enums;

namespace AzDevOps.Service.Infrastructure.Builders.Wiql;

public class WiqlBuilder : IWiqlBuilder
{
    private List<WiqlQueryComponent> queryComponents = new List<WiqlQueryComponent>();

    public void Reset()
    {
        queryComponents.Clear();
    }

    public IWiqlBuilder AddFilterOnlyByMyWorkItems()
    {
        queryComponents.Add(new(
            WiqlQueryComponentTypeEnum.FilterOnlyByMyWorkItems,
            "[System.AssignedTo]=@Me"));

        return this;
    }

    public IWiqlBuilder AddPagingByIds(SortEnum sortType, int currentPage, int nextPage, long firstId, long lastId)
    {
        string pagingFilter = "";
        if (nextPage > 0)
        {
            if (nextPage == currentPage)
            {
                if (sortType == SortEnum.Asc)
                    pagingFilter = $"id >= {firstId}";
                else
                    pagingFilter = $"id <= {firstId}";
            }
            else if (nextPage > currentPage)
            {
                if (sortType == SortEnum.Asc)
                    pagingFilter = $"id > {lastId}";
                else
                    pagingFilter = $"id < {lastId}";
            }
            else
            {
                if (sortType == SortEnum.Asc)
                    pagingFilter = $"id < {firstId}";
                else
                    pagingFilter = $"id > {firstId}";
            }
        }

        queryComponents.Add(new(
            WiqlQueryComponentTypeEnum.PagingByIdsFilters,
            pagingFilter,
            linkedQueryComponent: new(WiqlQueryComponentTypeEnum.Sort,
                $"id {sortType}")));

        return this;
    }

    public string Build()
    {
        var order = string.Empty;
        var where = string.Empty;
        var filterOnlyByMyWorkItems = BuildFilterOnlyByMyWorkItems();
        var pagingByIds = BuildPagingByIds();
        var whereFilters = new List<string>()
        {
            filterOnlyByMyWorkItems,
            pagingByIds?.WhereClauseFilter ?? string.Empty
        };

        foreach(var filter in whereFilters)
        {
            if (!string.IsNullOrEmpty(filter))
                where += $"{(string.IsNullOrEmpty(where) ? "where" : " and ")} {filter}";
        }

        order = pagingByIds?.OrderClause ?? string.Empty;
        return $"Select id From WorkItems {where} {order}";
    }

    private (string WhereClauseFilter, string OrderClause)? BuildPagingByIds()
    {
        var filter = queryComponents
                    .LastOrDefault(it => it.Type == WiqlQueryComponentTypeEnum.PagingByIdsFilters);
        if (filter != null)
        {
            var whereClauseFilter = filter.WiqlCode;
            if (filter.LinkedQueryComponent == null || filter.LinkedQueryComponent.Type != WiqlQueryComponentTypeEnum.Sort)
                throw new ArgumentException("Incorrect PagingByIdsFilters: it must be linked with a sort clause");

            var orderClause = $"order by {filter.LinkedQueryComponent.WiqlCode}";

            return (whereClauseFilter, orderClause);
        }

        return null;
    }

    private string BuildFilterOnlyByMyWorkItems()
    {
        var filter = queryComponents
                    .LastOrDefault(it => it.Type == WiqlQueryComponentTypeEnum.FilterOnlyByMyWorkItems);
        return filter?.WiqlCode ?? string.Empty;
    }
}
