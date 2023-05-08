using System;
using System.Collections.Generic;
using KitchenWeb.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace KitchenWeb.Repository;

public partial class KitchenDbContext : DbContext
{
    public KitchenDbContext()
    {
    }

    public KitchenDbContext(DbContextOptions<KitchenDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Budget> Budgets { get; set; }

    public virtual DbSet<EmloyeesPost> EmloyeesPosts { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Ingr> Ingrs { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<IngredientsAll> IngredientsAlls { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<MaterialOfUnit> MaterialOfUnits { get; set; }

    public virtual DbSet<MaterialsUnit> MaterialsUnits { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Production> Productions { get; set; }

    public virtual DbSet<ProductionView> ProductionViews { get; set; }

    public virtual DbSet<ProductionView1> ProductionView1s { get; set; }

    public virtual DbSet<ProductsUnit> ProductsUnits { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<PurchaseMaterial> PurchaseMaterials { get; set; }

    public virtual DbSet<PurchaseMaterialsView> PurchaseMaterialsViews { get; set; }

    public virtual DbSet<SalareView> SalareViews { get; set; }

    public virtual DbSet<Salary> Salaries { get; set; }

    public virtual DbSet<SaleProduct> SaleProducts { get; set; }

    public virtual DbSet<SaleProductsView> SaleProductsViews { get; set; }

    public virtual DbSet<UnitsOfMeasurement> UnitsOfMeasurements { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=HOME-PC;Initial Catalog=FinishKitchen;Trust Server Certificate=True;Integrated Security=True;User ID = sa;Password=1111;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmloyeesPost>(entity =>
        {
            entity.ToView("EmloyeesPost");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasOne(d => d.PostNavigation).WithMany(p => p.Employees).HasConstraintName("FK_Employees_Positions");
        });

        modelBuilder.Entity<Ingr>(entity =>
        {
            entity.ToView("Ingr");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.ToTable(tb =>
                {
                    tb.HasTrigger("price");
                    tb.HasTrigger("priceDelete");
                    tb.HasTrigger("priceUpdate");
                });

            entity.HasOne(d => d.MaterialNavigation).WithMany(p => p.Ingredients).HasConstraintName("FK_Ingredients_Materials");

            entity.HasOne(d => d.ProductNavigation).WithMany(p => p.Ingredients).HasConstraintName("FK_Ingredients_Products");
        });

        modelBuilder.Entity<IngredientsAll>(entity =>
        {
            entity.ToView("IngredientsAll");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasOne(d => d.UnitOfMeasurementNavigation).WithMany(p => p.Materials).HasConstraintName("FK_Materials_UnitsOfMeasurement");
        });

        modelBuilder.Entity<MaterialOfUnit>(entity =>
        {
            entity.ToView("MaterialOfUnit");
        });

        modelBuilder.Entity<MaterialsUnit>(entity =>
        {
            entity.ToView("MaterialsUnit");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasOne(d => d.UnitOfmeasurementNavigation).WithMany(p => p.Products).HasConstraintName("FK_Products_UnitsOfMeasurement");
        });

        modelBuilder.Entity<Production>(entity =>
        {
            entity.ToTable("Production", tb => tb.HasTrigger("ProductionInsert"));

            entity.HasOne(d => d.EmployeeNavigation).WithMany(p => p.Productions).HasConstraintName("FK_Production_Employees");

            entity.HasOne(d => d.ProductNavigation).WithMany(p => p.Productions).HasConstraintName("FK_Production_Products");
        });

        modelBuilder.Entity<ProductionView>(entity =>
        {
            entity.ToView("ProductionView");
        });

        modelBuilder.Entity<ProductionView1>(entity =>
        {
            entity.ToView("productionView1");
        });

        modelBuilder.Entity<ProductsUnit>(entity =>
        {
            entity.ToView("ProductsUnit");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.ToView("purchase");
        });

        modelBuilder.Entity<PurchaseMaterial>(entity =>
        {
            entity.ToTable("Purchase_materials", tb =>
                {
                    tb.HasTrigger("PurchaseDelete");
                    tb.HasTrigger("PurchaseInsert");
                    tb.HasTrigger("PurchaseUpdate");
                });

            entity.HasOne(d => d.EmployeeNavigation).WithMany(p => p.PurchaseMaterials).HasConstraintName("FK_Purchase_materials_Employees");

            entity.HasOne(d => d.MaterialNavigation).WithMany(p => p.PurchaseMaterials).HasConstraintName("FK_Purchase_materials_Materials");
        });

        modelBuilder.Entity<PurchaseMaterialsView>(entity =>
        {
            entity.ToView("Purchase_materialsView");
        });

        modelBuilder.Entity<SalareView>(entity =>
        {
            entity.ToView("salare_view");
        });

        modelBuilder.Entity<Salary>(entity =>
        {
            entity.HasOne(d => d.Employee).WithMany(p => p.Salaries).HasConstraintName("FK_Salary_Employees");
        });

        modelBuilder.Entity<SaleProduct>(entity =>
        {
            entity.ToTable("Sale_products", tb =>
                {
                    tb.HasTrigger("SaleDelete");
                    tb.HasTrigger("SaleInsert");
                    tb.HasTrigger("SaleUpdate");
                });

            entity.HasOne(d => d.EmployeeNavigation).WithMany(p => p.SaleProducts).HasConstraintName("FK_Sale_products_Employees");

            entity.HasOne(d => d.ProductNavigation).WithMany(p => p.SaleProducts).HasConstraintName("FK_Sale_products_Products");
        });

        modelBuilder.Entity<SaleProductsView>(entity =>
        {
            entity.ToView("Sale_productsView");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
