using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootWear.Model;

public partial class Good
{
    public string Artikle { get; set; } = null!;

    public string? NameGood { get; set; }

    public string? Unit { get; set; }

    public decimal? Price { get; set; }

    public int? Supplier { get; set; }

    public int? Manufacturer { get; set; }

    public int? Category { get; set; }

    public int? CurrentDiscount { get; set; }

    public int? AmountOnStorage { get; set; }

    public string? Description { get; set; }

    public byte[]? Photo { get; set; }

    public string CurrentDiscountS => $"{CurrentDiscount}%";

    [NotMapped]
    public bool hasDiscount { get; set; } = false;


    public string priceFormated 
    {
        get 
        {
            string a = string.Empty;
            if (CurrentDiscount > 0)
            {
                a = $"{Price - Price * CurrentDiscount/100}";
                
            }
            return a; 
        }
    }

    public virtual Category? CategoryNavigation { get; set; }

    public virtual Manufacturer? ManufacturerNavigation { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Supplier? SupplierNavigation { get; set; }
}
