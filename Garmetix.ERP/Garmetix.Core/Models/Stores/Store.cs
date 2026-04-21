using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Garmetix.Core.Models.Stores
{
     

    public class Company : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; } = DateTime.Now.Date;
        public DateTime? EndDate { get; set; }
        public bool Active { get; set; } = false;
        public string ContactNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = "Dumka";
        public string State { get; set; } = "Jharkhand";
        public string Country { get; set; } = "India";
        public string ZipCode { get; set; } = "814101";
        public string GSTIN { get; set; } = string.Empty;
        public string Pan { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public StoreCategory StoreCategory { get; set; } = StoreCategory.Retail;
        public string ContactPerson { get; set; } = string.Empty;
        public string ContactMobile { get; set; } = string.Empty;
        public string CIN { get; set; } = string.Empty;
        public CompanyType CompanyType { get; set; } = CompanyType.Proprietorship;

    }
    public class StoreGroup : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string GroupCode { get; set; } = string.Empty;
        public StoreCategory StoreCategory { get; set; } = StoreCategory.Retail;
        public DateTime StartDate { get; set; } = DateTime.Now.Date;
        public DateTime? EndDate { get; set; }
        public bool Active { get; set; } = false;
        [ForeignKey("Company")]
        public Guid CompanyId { get; set; }
        public virtual Company? Company { get; set; }
    }
    public class Store : BaseEntity
    {

        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; } = DateTime.Now.Date;
        public DateTime? EndDate { get; set; }
        public bool Active { get; set; } = false;
        public string StoreCode { get; set; } = string.Empty;
        public StoreCategory StoreCategory { get; set; } = StoreCategory.Retail;

        public string ContactNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = "Dumka";
        public string State { get; set; } = "Jharkhand";
        public string Country { get; set; } = "India";
        public string ZipCode { get; set; } = "814101";
        [ForeignKey("Company")]
        public Guid CompanyId { get; set; }

        [ForeignKey("StoreGroup")]
        public Guid StoreGroupId { get; set; }

        public virtual Company? Company { get; set; }
        public virtual StoreGroup? StoreGroup { get; set; }
    }
}
