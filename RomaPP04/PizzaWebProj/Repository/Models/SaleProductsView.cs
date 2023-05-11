using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KitchenWeb.Repository.Models;

[Keyless]
public partial class SaleProductsView
{
    [Column("ID")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(30)]
    public string? Name { get; set; }

    [Column("count")]
    public int? Count { get; set; }

    [Column("price")]
    public int? Price { get; set; }

    public int? Total { get; set; }

    [Column("date", TypeName = "date")]
    public DateTime? Date { get; set; }

    [Column("employee")]
    [StringLength(30)]
    public string? Employee { get; set; }
}
