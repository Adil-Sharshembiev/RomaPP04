using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KitchenWeb.Repository.Models;

[Keyless]
public partial class ProductsUnit
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(30)]
    public string? Name { get; set; }

    [Column("unit")]
    [StringLength(20)]
    public string? Unit { get; set; }

    [Column("count")]
    public int? Count { get; set; }

    [Column("price")]
    public double? Price { get; set; }

    [Column("sum")]
    public double? Sum { get; set; }

    [Column("percents")]
    public double? Percents { get; set; }
}
