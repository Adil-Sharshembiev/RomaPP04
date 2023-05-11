using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KitchenWeb.Repository.Models;

[Keyless]
public partial class SalareView
{
    [Column("name")]
    [StringLength(30)]
    public string? Name { get; set; }

    [Column("year")]
    public int? Year { get; set; }

    [Column("month")]
    public int? Month { get; set; }

    [Column("purchase_Count")]
    public int? PurchaseCount { get; set; }

    [Column("production_Count")]
    public int? ProductionCount { get; set; }

    [Column("sale_Count")]
    public int? SaleCount { get; set; }

    [Column("sum_Count")]
    public int? SumCount { get; set; }

    [Column("salary")]
    public double? Salary { get; set; }

    [Column("pay")]
    public bool? Pay { get; set; }
}
