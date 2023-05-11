using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KitchenWeb.Repository.Models;

[Keyless]
public partial class ProductionView1
{
    [Column("name")]
    [StringLength(30)]
    public string? Name { get; set; }

    [Column("count")]
    public int? Count { get; set; }

    [Column("date", TypeName = "date")]
    public DateTime? Date { get; set; }

    [Column("empl")]
    [StringLength(30)]
    public string? Empl { get; set; }
}
