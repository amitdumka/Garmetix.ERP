using Garmetix.Core.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garmetix.Core.Models
{
    public class CEntity : IEntity
    {
        [Display(AutoGenerateField = false)]
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
    public abstract class BaseEntity : IEntity
    {
        [Display(AutoGenerateField = false)]
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Display(AutoGenerateField = false)]
        // Audit Trail
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Display(AutoGenerateField = false)]
        public DateTime? UpdatedAt { get; set; }
        [Display(AutoGenerateField = false)]
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
        [Display(AutoGenerateField = false)]
        public Guid? CompanyId { get; set; }
        [NotMapped]
        public string? CompanyName { get; set; }= string.Empty;
    }

    // Level 2: Regional or Group Level Data
    public abstract class StoreGroupBase : CompanyBase
    {
        // Example for future:
        [Display(AutoGenerateField = false)]
        public Guid? StoreGroupId { get; set; }
        [NotMapped]
        public string? StoreName { get; set; }=string.Empty;

    }

    // Level 3: Individual Store/Branch Data
    public abstract class StoreBase : StoreGroupBase
    {
        // Example for future:
        [Display(AutoGenerateField = false)]
        public Guid? StoreId { get; set; }

        [NotMapped]
        public string? StoreLocation { get; set; }=string.Empty;
    }
}