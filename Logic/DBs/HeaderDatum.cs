using System;
using System.Collections.Generic;

namespace Logic;

public partial class HeaderDatum
{
    public int? ManagerId { get; set; }

    public string Title { get; set; } = null!;

    public string Slogan { get; set; } = null!;

    public string ColorBackroundHeader { get; set; } = null!;

    public string ColorFont { get; set; } = null!;

    public byte[] LogoContent { get; set; } = null!;

    public int Id { get; set; }

    public string FileName { get; set; } = null!;

    public virtual User? Manager { get; set; }
}
