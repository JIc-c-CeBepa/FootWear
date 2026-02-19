using System;
using System.Collections.Generic;

namespace FootWear.Model;

public partial class Category
{
    public int Idcategory { get; set; }

    public string? NameCategory { get; set; }

    public virtual ICollection<Good> Goods { get; set; } = new List<Good>();
}
