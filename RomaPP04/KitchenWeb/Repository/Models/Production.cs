using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KitchenWeb.Repository.Models;

[Table("Production")]
public partial class Production
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("product")]
    public int? Product { get; set; }

    [Column("count")]
    public int? Count { get; set; }

    [Column("date", TypeName = "date")]
    public DateTime? Date { get; set; }

    [Column("employee")]
    public int? Employee { get; set; }

    [ForeignKey("Employee")]
    [InverseProperty("Productions")]
    public virtual Employee? EmployeeNavigation { get; set; }

    [ForeignKey("Product")]
    [InverseProperty("Productions")]
    public virtual Product? ProductNavigation { get; set; }
}
