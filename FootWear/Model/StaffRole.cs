using System;
using System.Collections.Generic;

namespace FootWear.Model;

public partial class StaffRole
{
    public int IdstaffRoles { get; set; }

    public string? NameRole { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
