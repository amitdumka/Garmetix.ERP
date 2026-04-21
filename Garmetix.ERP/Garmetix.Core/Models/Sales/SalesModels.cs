using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Garmetix.Core.Models;

namespace Garmetix.Core.Models.Sales
{
    public class Product : CompanyBase
    {
        [Display(Name = "Item Code / Barcode")]
        [Required]
        public string Barcode { get; set; } = string.Empty;

        [Display(Name = "Product Name")]
        [Required]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Selling Price (₹)")]
        public decimal SellingPrice { get; set; }

        [Display(Name = "Tax / GST %")]
        public decimal TaxPercentage { get; set; } = 0;
    }

    public class Stock : StoreBase
    {
        [Display(AutoGenerateField = false)]
        public Guid ProductId { get; set; }
        
        [JsonIgnore]
        [Display(AutoGenerateField = false)]
        public virtual Product? Product { get; set; }

        [Display(Name = "Current Available Quantity")]
        public decimal CurrentQty { get; set; }
    }

    public class Invoice : StoreBase
    {
        [Display(Name = "Invoice Number")]
        public string InvoiceNo { get; set; } = string.Empty;

        public DateTime InvoiceDate { get; set; } = DateTime.Now;

        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; } = "Cash Customer";

        [Display(Name = "Customer Mobile")]
        public string? MobileNo { get; set; }

        public decimal SubTotal { get; set; }
        public decimal TotalTax { get; set; }
        public decimal GrandTotal { get; set; }
        
        public PaymentMode PaymentMode { get; set; } = PaymentMode.Cash;
    }

    public class InvoiceItem : BaseEntity
    {
        public Guid InvoiceId { get; set; }
        public Guid ProductId { get; set; }
        
        public string ProductName { get; set; } = string.Empty;
        public decimal Rate { get; set; }
        public decimal Quantity { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}