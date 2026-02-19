using System;
using System.Collections.Generic;

namespace FootWear.Model;

public partial class OrderItem
{
    public int IdorderItems { get; set; }

    public int? IdOrder { get; set; }

    public string? Articke { get; set; }

    public int? Amount { get; set; }

    public virtual Good? ArtickeNavigation { get; set; }

    public virtual Order? IdOrderNavigation { get; set; }
}
