using System;
using System.Collections.Generic;

namespace FootWear.Model;

public partial class Manufacturer
{
    public int Idmanufacturer { get; set; }

    public string? NameManufacturer { get; set; }

    public virtual ICollection<Good> Goods { get; set; } = new List<Good>();
}
