namespace Checkout.DataAccess.Models.Base;

using System;

public abstract class BaseAuditEntity : BaseEntity
{
    public DateTime CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual void SetAsCreated(string userId = "User")
    {
        if (string.IsNullOrWhiteSpace(CreatedBy))
        {
            CreatedOn = DateTime.UtcNow;
            CreatedBy = userId;
        }
    }

    public virtual void SetAsModified(string userId = "User")
    {
        if (Id > 0)
        {
            ModifiedOn = DateTime.UtcNow;
            ModifiedBy = userId;
        }
        else
        {
            SetAsCreated(userId);
        }
    }
}