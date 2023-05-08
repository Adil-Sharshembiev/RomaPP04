using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KitchenWeb.Repository.Models;

[Table("UnitsOfMeasurement")]
public partial class UnitsOfMeasurement
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(20)]
    public string? Name { get; set; }

    [InverseProperty("UnitOfMeasurementNavigation")]
    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();

    [InverseProperty("UnitOfmeasurementNavigation")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
