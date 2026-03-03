using System;
using System.Collections.Generic;
using System.IO;

namespace FootWear.Model;

public partial class Order
{
    public int Idorder { get; set; }

    public DateOnly? DateStartOrder { get; set; }

    public DateOnly? DateDeliver { get; set; }

    public int? PickUpPointAdress { get; set; }

    public int? ClientId { get; set; }

    public string? RecieveCode { get; set; }

    public string? Status { get; set; }

    public virtual User? Client { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual PickUpPoint? PickUpPointAdressNavigation { get; set; }

    public string ArtikleList
    {
        get
        {
            

            return string.Join(", ",
                OrderItems
                    .Where(i => !string.IsNullOrWhiteSpace(i.Articke))
                    .Select(i => i.Articke));
        }
    }

}
