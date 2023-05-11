using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KitchenWeb.Repository.Models;

public partial class Product
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(30)]
    public string? Name { get; set; }

    [Column("unitOfmeasurement")]
    public int? UnitOfmeasurement { get; set; }

    [Column("count")]
    public int? Count { get; set; }

    [Column("price")]
    public double? Price { get; set; }

    [Column("sum")]
    public double? Sum { get; set; }

    [Column("percents")]
    public double? Percents { get; set; }

    [InverseProperty("ProductNavigation")]
    public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    [InverseProperty("ProductNavigation")]
    public virtual ICollection<Production> Productions { get; set; } = new List<Production>();

    [InverseProperty("ProductNavigation")]
    public virtual ICollection<SaleProduct> SaleProducts { get; set; } = new List<SaleProduct>();

    [ForeignKey("UnitOfmeasurement")]
    [InverseProperty("Products")]
    public virtual UnitsOfMeasurement? UnitOfmeasurementNavigation { get; set; }
}
