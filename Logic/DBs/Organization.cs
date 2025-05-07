using System;
using System.Collections.Generic;

namespace Logic;

public partial class Organization
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string Email { get; set; } = null!;

    public string? About { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
