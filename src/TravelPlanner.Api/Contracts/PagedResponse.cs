namespace TravelPlanner.Api.Contracts;

public class PagedResponse<T>
{
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
    public IEnumerable<T> Data { get; init; } = new List<T>();
}