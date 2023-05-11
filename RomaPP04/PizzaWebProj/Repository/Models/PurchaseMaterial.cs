using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KitchenWeb.Repository.Models;

[Table("Purchase_materials")]
public partial class PurchaseMaterial
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("material")]
    public int? Material { get; set; }

    [Column("count")]
    public double? Count { get; set; }

    [Column("price")]
    public double? Price { get; set; }

    [Column("date", TypeName = "date")]
    public DateTime? Date { get; set; }

    [Column("employee")]
    public int? Employee { get; set; }

    [ForeignKey("Employee")]
    [InverseProperty("PurchaseMaterials")]
    public virtual Employee? EmployeeNavigation { get; set; }

    [ForeignKey("Material")]
    [InverseProperty("PurchaseMaterials")]
    public virtual Material? MaterialNavigation { get; set; }
}
