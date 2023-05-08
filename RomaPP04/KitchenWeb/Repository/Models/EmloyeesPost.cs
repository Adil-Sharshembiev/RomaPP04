using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KitchenWeb.Repository.Models;

[Keyless]
public partial class EmloyeesPost
{
    [Column("ID")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(30)]
    public string? Name { get; set; }

    [Column("post")]
    [StringLength(30)]
    public string? Post { get; set; }

    [Column("salary")]
    public int? Salary { get; set; }

    [Column("address")]
    [StringLength(40)]
    public string? Address { get; set; }

    [Column("phone")]
    [StringLength(12)]
    [Unicode(false)]
    public string? Phone { get; set; }
}
