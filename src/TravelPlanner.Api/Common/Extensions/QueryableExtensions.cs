using System.Linq.Expressions;
using TravelPlanner.Api.Common.Models;
using TravelPlanner.Api.Common.Models.Enums;

namespace TravelPlanner.Api.Common.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> OrderBy<T>(
        this IQueryable<T> query,
        Expression<Func<T, object>> keySelector,
        SortDirection sortDirection)
    {
        return sortDirection == SortDirection.Ascending
            ? query.OrderBy(keySelector)
            : query.OrderByDescending(keySelector);
    }
    
    public static IQueryable<T> Paginate<T>(
        this IQueryable<T> query,
        PaginationQuery paginationQuery)
    {
        return query.Skip((paginationQuery.PageIndex - 1) * paginationQuery.PageSize)
            .Take(paginationQuery.PageSize);
    }
}