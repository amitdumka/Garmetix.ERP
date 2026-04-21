using Garmetix.Core.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace Garmetix.Core.Models
{
    public abstract class BaseEntity : IEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // Audit Trail
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }

        // Data Status Flags
        public bool Synced { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}

namespace Garmetix.Core.Models
{
    // Level 1: Company Level Data
    public abstract class CompanyBase : BaseEntity
    {
        // Example for future: public Guid? CompanyId { get; set; } 
    }

    // Level 2: Regional or Group Level Data
    public abstract class StoreGroupBase : CompanyBase
    {
        // Example for future: public Guid? StoreGroupId { get; set; }
    }

    // Level 3: Individual Store/Branch Data
    public abstract class StoreBase : StoreGroupBase
    {
        // Example for future: public Guid? StoreId { get; set; }
    }
}