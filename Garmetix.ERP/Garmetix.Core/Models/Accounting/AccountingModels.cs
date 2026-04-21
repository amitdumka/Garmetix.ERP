using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Garmetix.Core.Models;

namespace Garmetix.Core.Models.Accounting
{
    // --- VOUCHER BASE & DERIVATIVES ---
    public class VoucherBase : StoreBase
    {
        public required string VoucherNumber { get; set; }
        public DateTime OnDate { get; set; }
        public VoucherType VoucherType { get; set; } = VoucherType.Payment;
        public required string PartyName { get; set; }
        public required string Particulars { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public string? SlipNumber { get; set; }

        public Guid? LedgerId { get; set; } = Guid.Empty;
        public virtual Ledger? Ledger { get; set; }
    }

    public class Voucher : VoucherBase
    {
        public PaymentMode PaymentMode { get; set; } = PaymentMode.Cash;
        public string? PaymentDetails { get; set; }
        public bool IsParty { get; set; } = false;

        public Guid? PartyId { get; set; } = Guid.Empty;
        public virtual Party? Party { get; set; }

        [ForeignKey("BankAccount")]
        public Guid? AccountNumber { get; set; }
        public virtual BankAccount? BankAccount { get; set; }
    }
    public class LedgerGroup : CompanyBase
    {
        [Display(Name = "Ledger Group Name", Prompt = "e.g., Sundry Creditors")]
        [Required(ErrorMessage = "Group name is required.")]
        public required string Name { get; set; }

        [Display(Name = "Category Classification")]
        public LedgerCategory Category { get; set; }

        [Display(Name = "Additional Remarks")]
        public string Remarks { get; set; } = string.Empty;
    }

    public class Ledger : CompanyBase
    {
        [Display(Name = "Ledger Name", Prompt = "Enter ledger account name")]
        [Required(ErrorMessage = "Ledger name is required.")]
        public required string Name { get; set; } = string.Empty;

        [Display(Name = "Ledger Type")]
        public LedgerType LedgerType { get; set; }

        [Display(Name = "Opening Date")]
        public DateTime OpenningDate { get; set; }

        [Display(Name = "Opening Balance (₹)")]
        public decimal OpenningBalance { get; set; }

        [Display(Name = "Is this a Party/Person?")]
        public bool IsParty { get; set; } = false;

        // Hidden Database Relational Fields
        [Display(AutoGenerateField = false)]
        public Guid LedgerGroupId { get; set; }

        [JsonIgnore]
        [Display(AutoGenerateField = false)]
        public virtual LedgerGroup? LedgerGroup { get; set; }
    }

    public class Party : CompanyBase
    {
        [Display(Name = "Party/Company Name", Prompt = "Enter full business name")]
        [Required(ErrorMessage = "Party Name is required.")]
        public required string Name { get; set; } = string.Empty;

        [Display(Name = "Contact Number", Prompt = "10-digit mobile number")]
        [Phone(ErrorMessage = "Invalid phone format.")]
        public string? Phone { get; set; }

        [Display(Name = "Email Address", Prompt = "contact@company.com")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? EmailId { get; set; }

        [Display(Name = "GSTIN Number", Prompt = "e.g., 20AAAAA0000A1Z5")]
        [StringLength(15, ErrorMessage = "GSTIN must be 15 characters.")]
        public string? GSTIN { get; set; }

        [Display(Name = "PAN Number", Prompt = "e.g., ABCDE1234F")]
        [StringLength(10, ErrorMessage = "PAN must be 10 characters.")]
        public string? PAN { get; set; }

        [Display(Name = "Party Type Classification")]
        public PartyType Category { get; set; }

        [Display(Name = "Full Billing Address")]
        [DataType(DataType.MultilineText)]
        public string? Address { get; set; }

        // Hidden Database Relational Fields
        [Display(AutoGenerateField = false)]
        public Guid LedgerId { get; set; } = Guid.Empty;

        [JsonIgnore]
        [Display(AutoGenerateField = false)]
        public virtual Ledger? Ledger { get; set; }
    }

    public class Bank : CompanyBase
    {
        [Display(Name = "Bank Institution Name", Prompt = "e.g., HDFC Bank, SBI")]
        [Required(ErrorMessage = "Bank Name is required.")]
        public string Name { get; set; } = string.Empty;
    }

    public class BankAccount : CompanyBase
    {
        [Display(Name = "Account Number")]
        [Required(ErrorMessage = "Account number is required.")]
        public string AccountNumber { get; set; } = string.Empty;

        [Display(Name = "Account Holder Name")]
        [Required(ErrorMessage = "Holder name is required.")]
        public string AccountHolderName { get; set; } = string.Empty;

        [Display(Name = "Account Type")]
        public AccountType AccountType { get; set; } = AccountType.Current;

        [Display(Name = "Branch Name", Prompt = "e.g., Main Branch, Dumka")]
        public string? Branch { get; set; }

        [Display(Name = "IFSC Code", Prompt = "e.g., HDFC0001234")]
        public string? IFSCode { get; set; }

        [Display(Name = "Opening Date")]
        public DateTime OpeningDate { get; set; } = DateTime.Now;

        [Display(Name = "Opening Balance (₹)")]
        public decimal OpeningBalance { get; set; } = 0;

        [Display(Name = "Is Account Active?")]
        public bool Active { get; set; } = true;

        // Hidden Database Relational Fields
        [Display(AutoGenerateField = false)]
        public DateTime? ClosingDate { get; set; }
        [Display(AutoGenerateField = false)]
        public decimal ClosingBalance { get; set; } = 0;
        [Display(AutoGenerateField = false)]
        public Guid BankId { get; set; }
        [Display(AutoGenerateField = false)]
        public virtual Bank? Bank { get; set; }
        [Display(AutoGenerateField = false)]
        public Guid LedgerId { get; set; }
        [Display(AutoGenerateField = false)]
        public virtual Ledger? Ledger { get; set; }
    }
}