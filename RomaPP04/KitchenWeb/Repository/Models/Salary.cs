using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KitchenWeb.Repository.Models;

[Table("Salary")]
public partial class Salary
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("employee_Id")]
    public int? EmployeeId { get; set; }

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
    public double? Salary1 { get; set; }

    [Column("pay")]
    public bool? Pay { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("Salaries")]
    public virtual Employee? Employee { get; set; }
}
