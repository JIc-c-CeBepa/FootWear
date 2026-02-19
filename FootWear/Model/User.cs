using System;
using System.Collections.Generic;

namespace FootWear.Model;

public partial class User
{
    public int Iduser { get; set; }

    public int? Role { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Patronomyic { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual StaffRole? RoleNavigation { get; set; }
}
