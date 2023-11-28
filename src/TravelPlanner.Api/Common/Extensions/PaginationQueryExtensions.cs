using TravelPlanner.Api.Common.Contracts;
using TravelPlanner.Api.Common.Models;

namespace TravelPlanner.Api.Common.Extensions;

public static class PaginationQueryExtensions
{
    public static PagedResponse<T> MapToPagedResponse<T>(this PaginationQuery query, IEnumerable<T> items, int totalItems)
    {
        return new PagedResponse<T>()
        {
            Data = items,
            PageIndex = query.PageIndex,
            PageSize = query.PageSize,
            TotalCount = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)query.PageSize)
        };
    }
}