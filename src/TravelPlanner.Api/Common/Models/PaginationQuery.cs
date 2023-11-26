using FluentValidation;
using TravelPlanner.Api.Common.Models.Enums;

namespace TravelPlanner.Api.Common.Models;

public abstract class PaginationQuery
{
    public required int PageIndex { get; init; }
    public required int PageSize { get; init; }
    public string SortBy { get; init; } = string.Empty;
    public SortDirection SortDirection { get; init; } = SortDirection.Ascending;
}

public class PaginationQueryValidator : AbstractValidator<PaginationQuery>
{
    public PaginationQueryValidator()
    {
        RuleFor(x => x.PageIndex).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
        
        RuleFor(x => x.SortDirection).IsInEnum();
    }
}