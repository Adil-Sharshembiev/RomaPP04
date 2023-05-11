using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KitchenWeb.Repository.Models;

public partial class Employee
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(30)]
    public string? Name { get; set; }

    [Column("post")]
    public int? Post { get; set; }

    [Column("salary")]
    public int? Salary { get; set; }

    [Column("address")]
    [StringLength(40)]
    public string? Address { get; set; }

    [Column("phone")]
    [StringLength(12)]
    [Unicode(false)]
    public string? Phone { get; set; }

    [ForeignKey("Post")]
    [InverseProperty("Employees")]
    public virtual Position? PostNavigation { get; set; }

    [InverseProperty("EmployeeNavigation")]
    public virtual ICollection<Production> Productions { get; set; } = new List<Production>();

    [InverseProperty("EmployeeNavigation")]
    public virtual ICollection<PurchaseMaterial> PurchaseMaterials { get; set; } = new List<PurchaseMaterial>();

    [InverseProperty("Employee")]
    public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();

    [InverseProperty("EmployeeNavigation")]
    public virtual ICollection<SaleProduct> SaleProducts { get; set; } = new List<SaleProduct>();
}
