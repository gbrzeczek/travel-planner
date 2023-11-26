using Enums = TravelPlanner.Api.Common.Models.Enums;

namespace TravelPlanner.Api.Contracts;

public class PaginationRequest
{
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
    public string? SortBy { get; init; } = string.Empty;
    public Enums.SortDirection? SortDirection { get; init; } = Enums.SortDirection.Ascending;
}