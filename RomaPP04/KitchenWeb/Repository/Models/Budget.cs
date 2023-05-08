using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KitchenWeb.Repository.Models;

[Table("Budget")]
public partial class Budget
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("budget")]
    public double? Budget1 { get; set; }

    [Column("bonus")]
    public int? Bonus { get; set; }
}
