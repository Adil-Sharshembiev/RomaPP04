using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KitchenWeb.Repository.Models;

[Keyless]
public partial class Purchase
{
    [Column("ID")]
    public int Id { get; set; }

    [Column("material")]
    public int? Material { get; set; }

    [Column("mat")]
    [StringLength(30)]
    public string? Mat { get; set; }

    [Column("count")]
    public double? Count { get; set; }

    [Column("price")]
    public double? Price { get; set; }

    [Column("date", TypeName = "date")]
    public DateTime? Date { get; set; }

    [Column("employee")]
    public int? Employee { get; set; }

    [Column("name")]
    [StringLength(30)]
    public string? Name { get; set; }
}
