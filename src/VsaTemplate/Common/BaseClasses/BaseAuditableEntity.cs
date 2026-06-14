namespace VsaTemplate.Common.BaseClasses;

// credit: Jason Taylor
public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTimeOffset CreatedOn { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTimeOffset LastModifiedOn { get; set; }
    public Guid? LastModifiedBy { get; set; }
}
