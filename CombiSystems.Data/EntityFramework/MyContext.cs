using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CombiSystems.Data.Identity;
using CombiSystems.Core.Entities;

namespace CombiSystems.Data.EntityFramework;

public sealed class MyContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public MyContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        //Fluent API
        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(50).IsRequired(false);
            entity.Property(x => x.Surname).HasMaxLength(50).IsRequired(false);
            entity.Property(x => x.PhoneNumber).HasMaxLength(10).IsRequired(false);
            entity.Property(x => x.RegisterDate).HasColumnType("datetime");
        });

        builder.Entity<ApplicationRole>(entity =>
        {
            entity.Property(x => x.Description).HasMaxLength(120).IsRequired(false);
        });


        builder.Entity<Category>(entity =>
        {
            entity.HasIndex(x => x.Id);
            entity.HasMany(x => x.Products)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId);
            entity.Property(x => x.Name).IsRequired().HasMaxLength(50);
            entity.Property(x => x.Description).IsRequired(false).HasMaxLength(250);
        });


        builder.Entity<Product>(entity =>
        {
            entity.HasIndex(x => x.Id);
            entity.HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId);
            entity.Property(x => x.Name).IsRequired().HasMaxLength(50);
            entity.Property(x => x.UnitPrice).HasPrecision(8, 2);
        });

        builder.Entity<Appointment>(entity =>
        {
            entity.HasIndex(x => x.Id);
            entity.HasOne(x => x.Bill)
                .WithOne(x => x.Appointment)
                .HasForeignKey<Bill>(x => x.AppointmentId);
            entity.Property(x => x.UserId).IsRequired();
            entity.Property(x => x.AppointentOpeningDate).IsRequired();
            entity.Property(x => x.TechnicianAssignDate).IsRequired();
            entity.Property(x => x.AppointentClosingDate).IsRequired();
            entity.Property(x => x.Description).IsRequired(false).HasMaxLength(250);
            entity.Property(x => x.TaskStatus).IsRequired();

        });

        builder.Entity<Bill>(entity =>
        {
            entity.HasIndex(x => x.Id);
            entity.HasOne(x => x.Appointment)
                .WithOne(x => x.Bill)
                .HasForeignKey<Bill>(x => x.AppointmentId);
            entity.Property(x => x.TotalPrice).IsRequired();
            entity.Property(x => x.PaymentStatus).IsRequired();
        });

        builder.Entity<BillDetails>(entity =>
        {
            entity.HasIndex(x => x.Id);
            entity.HasOne(x => x.Bill)
                .WithMany(x => x.BillDetails)
                .HasForeignKey(x => x.BillId);
            entity.HasOne(x => x.Product)
                .WithOne(x => x.BillDetails)
                .HasForeignKey<BillDetails>(x => x.ProductId);
            entity.Property(x => x.ProductId).IsRequired();
            entity.Property(x => x.SalesAmount).IsRequired();
            entity.Property(x => x.Count).IsRequired();
        });
    }

    public DbSet<Category> Categories
    {
        get; set;
    }
    public DbSet<Product> Products
    {
        get; set;
    }

    public DbSet<Appointment> Appointments
    {
        get; set;
    }
    public DbSet<Bill> Bills
    {
        get; set;
    }
    public DbSet<BillDetails> BillDetails
    {
        get; set;
    }
}