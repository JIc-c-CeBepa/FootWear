using System;
using System.Collections.Generic;

namespace FootWear.Model;

public partial class PickUpPoint
{
    public int IdPickUpPint { get; set; }

    public string? PostIndex { get; set; }

    public string? City { get; set; }

    public string? Street { get; set; }

    public string? HouseNum { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
