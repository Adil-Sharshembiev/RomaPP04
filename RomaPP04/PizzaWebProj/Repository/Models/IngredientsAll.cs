using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KitchenWeb.Repository.Models;

[Keyless]
public partial class IngredientsAll
{
    [Column("product")]
    [StringLength(30)]
    public string? Product { get; set; }

    [Column("material")]
    [StringLength(30)]
    public string? Material { get; set; }

    [Column("count")]
    public double? Count { get; set; }
}
