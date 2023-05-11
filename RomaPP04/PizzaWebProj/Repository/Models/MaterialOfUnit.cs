using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KitchenWeb.Repository.Models;

[Keyless]
public partial class MaterialOfUnit
{
    [Column("ID")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(30)]
    public string? Name { get; set; }

    [Column("unit")]
    [StringLength(20)]
    public string? Unit { get; set; }

    [Column("count")]
    public double? Count { get; set; }

    [Column("price")]
    public double? Price { get; set; }

    [Column("cost")]
    public double? Cost { get; set; }
}
