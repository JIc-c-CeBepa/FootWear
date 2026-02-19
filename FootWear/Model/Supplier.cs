using System;
using System.Collections.Generic;

namespace FootWear.Model;

public partial class Supplier
{
    public int Idsupplier { get; set; }

    public string? NameSupplier { get; set; }

    public virtual ICollection<Good> Goods { get; set; } = new List<Good>();
}
