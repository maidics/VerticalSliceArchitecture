namespace VsaTemplate.Common.BaseClasses;

// credit to: Jason Taylor
public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTimeOffset Created { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTimeOffset LastModified { get; set; }
    public Guid? ModifiedBy { get; set; }
}
