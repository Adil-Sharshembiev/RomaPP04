using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KitchenWeb.Repository.Models;

[Table("Sale_products")]
public partial class SaleProduct
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("product")]
    public int? Product { get; set; }

    [Column("count")]
    public int? Count { get; set; }

    [Column("price")]
    public int? Price { get; set; }

    [Column("sum")]
    public int? Sum { get; set; }

    [Column("date", TypeName = "date")]
    public DateTime? Date { get; set; }

    [Column("employee")]
    public int? Employee { get; set; }

    [ForeignKey("Employee")]
    [InverseProperty("SaleProducts")]
    public virtual Employee? EmployeeNavigation { get; set; }

    [ForeignKey("Product")]
    [InverseProperty("SaleProducts")]
    public virtual Product? ProductNavigation { get; set; }
}
