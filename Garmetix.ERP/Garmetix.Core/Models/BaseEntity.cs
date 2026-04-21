using System;
using System.ComponentModel.DataAnnotations;

namespace Garmetix.Core.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        // Universal Audit Trail
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Critical for Enterprise Systems: Never actually delete financial data
        public bool IsDeleted { get; set; } = false; 
    }
}

namespace Garmetix.Core.Models
{
    public abstract class CompanyBase : BaseEntity
    {
        // Future proofing for Multi-Branch/Multi-Company support
        // public Guid? CompanyId { get; set; } 
    }

    public abstract class StoreBase : CompanyBase
    {
        // Future proofing for Multi-Store POS support
        // public Guid? StoreId { get; set; }
    }
}