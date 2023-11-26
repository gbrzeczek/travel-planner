namespace TravelPlanner.Api.Common;

public abstract class AuditableEntity
{
    public DateTimeOffset Created { get; set; }
    public int? CreatedBy { get; set; }
    public DateTimeOffset? LastModified { get; set; }
    public int? LastModifiedBy { get; set; }
}