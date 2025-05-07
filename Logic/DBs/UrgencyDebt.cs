﻿using System;
using System.Collections.Generic;

namespace Logic;

public partial class UrgencyDebt
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<Debt> Debts { get; set; } = new List<Debt>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
