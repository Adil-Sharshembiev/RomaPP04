using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KitchenWeb.Repository.Models;

[Index("Name", Name = "IX_Materials", IsUnique = true)]
public partial class Material
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(30)]
    public string? Name { get; set; }

    [Column("unitOfMeasurement")]
    public int? UnitOfMeasurement { get; set; }

    [Column("count")]
    public double? Count { get; set; }

    [Column("price")]
    public double? Price { get; set; }

    [Column("cost")]
    public double? Cost { get; set; }

    [InverseProperty("MaterialNavigation")]
    public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    [InverseProperty("MaterialNavigation")]
    public virtual ICollection<PurchaseMaterial> PurchaseMaterials { get; set; } = new List<PurchaseMaterial>();

    [ForeignKey("UnitOfMeasurement")]
    [InverseProperty("Materials")]
    public virtual UnitsOfMeasurement? UnitOfMeasurementNavigation { get; set; }
}
