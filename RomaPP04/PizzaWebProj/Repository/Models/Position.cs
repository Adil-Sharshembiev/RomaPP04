using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KitchenWeb.Repository.Models;

public partial class Position
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("post")]
    [StringLength(30)]
    public string? Post { get; set; }

    [InverseProperty("PostNavigation")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
