using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace FootWear.Model;

public partial class FootwearContext : DbContext
{
    public static FootwearContext db { get; private set; } = new FootwearContext();
    public FootwearContext()
    {
    }

    public FootwearContext(DbContextOptions<FootwearContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Good> Goods { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<PickUpPoint> PickUpPoints { get; set; }

    public virtual DbSet<StaffRole> StaffRoles { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=2314;database=footwear", Microsoft.EntityFrameworkCore.ServerVersion.Parse("9.3.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Idcategory).HasName("PRIMARY");

            entity.ToTable("category");

            entity.Property(e => e.Idcategory).HasColumnName("idcategory");
            entity.Property(e => e.NameCategory)
                .HasMaxLength(45)
                .HasColumnName("name_category");
        });

        modelBuilder.Entity<Good>(entity =>
        {
            entity.HasKey(e => e.Artikle).HasName("PRIMARY");

            entity.ToTable("goods");

            entity.HasIndex(e => e.Category, "cat_idx");

            entity.HasIndex(e => e.Manufacturer, "man_idx");

            entity.HasIndex(e => e.Supplier, "sup_idx");

            entity.Property(e => e.Artikle)
                .HasMaxLength(6)
                .HasColumnName("artikle");
            entity.Property(e => e.AmountOnStorage).HasColumnName("amount_on_storage");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.CurrentDiscount).HasColumnName("current_discount");
            entity.Property(e => e.Description)
                .HasMaxLength(144)
                .HasColumnName("description");
            entity.Property(e => e.Manufacturer).HasColumnName("manufacturer");
            entity.Property(e => e.NameGood).HasColumnName("name_good");
            entity.Property(e => e.Photo).HasColumnName("photo");
            entity.Property(e => e.Price)
                .HasPrecision(8, 2)
                .HasColumnName("price");
            entity.Property(e => e.Supplier).HasColumnName("supplier");
            entity.Property(e => e.Unit)
                .HasMaxLength(3)
                .HasColumnName("unit");

            entity.HasOne(d => d.CategoryNavigation).WithMany(p => p.Goods)
                .HasForeignKey(d => d.Category)
                .HasConstraintName("cat");

            entity.HasOne(d => d.ManufacturerNavigation).WithMany(p => p.Goods)
                .HasForeignKey(d => d.Manufacturer)
                .HasConstraintName("man");

            entity.HasOne(d => d.SupplierNavigation).WithMany(p => p.Goods)
                .HasForeignKey(d => d.Supplier)
                .HasConstraintName("sup");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Idmanufacturer).HasName("PRIMARY");

            entity.ToTable("manufacturer");

            entity.Property(e => e.Idmanufacturer).HasColumnName("idmanufacturer");
            entity.Property(e => e.NameManufacturer)
                .HasMaxLength(45)
                .HasColumnName("name_manufacturer");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Idorder).HasName("PRIMARY");

            entity.ToTable("order");

            entity.HasIndex(e => e.ClientId, "client_idx");

            entity.HasIndex(e => e.PickUpPointAdress, "pick_idx");

            entity.Property(e => e.Idorder).HasColumnName("idorder");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.DateDeliver).HasColumnName("date_deliver");
            entity.Property(e => e.DateStartOrder).HasColumnName("date_start_order");
            entity.Property(e => e.PickUpPointAdress).HasColumnName("pick_up_point_adress");
            entity.Property(e => e.RecieveCode)
                .HasMaxLength(3)
                .HasColumnName("recieve_code");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasColumnName("status");

            entity.HasOne(d => d.Client).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("client");

            entity.HasOne(d => d.PickUpPointAdressNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PickUpPointAdress)
                .HasConstraintName("pick");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.IdorderItems).HasName("PRIMARY");

            entity.ToTable("order_items");

            entity.HasIndex(e => e.Articke, "good_idx");

            entity.HasIndex(e => e.IdOrder, "order_idx");

            entity.Property(e => e.IdorderItems).HasColumnName("idorder_items");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Articke)
                .HasMaxLength(6)
                .HasColumnName("articke");
            entity.Property(e => e.IdOrder).HasColumnName("id_order");

            entity.HasOne(d => d.ArtickeNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.Articke)
                .HasConstraintName("asd");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.IdOrder)
                .HasConstraintName("order");
        });

        modelBuilder.Entity<PickUpPoint>(entity =>
        {
            entity.HasKey(e => e.IdPickUpPint).HasName("PRIMARY");

            entity.ToTable("pick_up_point");

            entity.Property(e => e.IdPickUpPint).HasColumnName("id_pick_up_pint");
            entity.Property(e => e.City)
                .HasMaxLength(45)
                .HasColumnName("city");
            entity.Property(e => e.HouseNum)
                .HasMaxLength(45)
                .HasColumnName("house_num");
            entity.Property(e => e.PostIndex)
                .HasMaxLength(6)
                .HasColumnName("post_index");
            entity.Property(e => e.Street)
                .HasMaxLength(45)
                .HasColumnName("street");
        });

        modelBuilder.Entity<StaffRole>(entity =>
        {
            entity.HasKey(e => e.IdstaffRoles).HasName("PRIMARY");

            entity.ToTable("staff_roles");

            entity.Property(e => e.IdstaffRoles).HasColumnName("idstaff_roles");
            entity.Property(e => e.NameRole)
                .HasMaxLength(45)
                .HasColumnName("name_role");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Idsupplier).HasName("PRIMARY");

            entity.ToTable("supplier");

            entity.Property(e => e.Idsupplier).HasColumnName("idsupplier");
            entity.Property(e => e.NameSupplier)
                .HasMaxLength(45)
                .HasColumnName("name_supplier");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Iduser).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.Role, "role_idx");

            entity.Property(e => e.Iduser).HasColumnName("iduser");
            entity.Property(e => e.FirstName)
                .HasMaxLength(45)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(45)
                .HasColumnName("last_name");
            entity.Property(e => e.Login)
                .HasMaxLength(45)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(15)
                .HasColumnName("password");
            entity.Property(e => e.Patronomyic)
                .HasMaxLength(45)
                .HasColumnName("patronomyic");
            entity.Property(e => e.Role).HasColumnName("role");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Role)
                .HasConstraintName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
