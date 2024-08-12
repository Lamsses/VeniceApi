using EFDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;


namespace EFDataAccessLibrary.DataAccess;

public class OTContext : DbContext
{
    public OTContext(DbContextOptions<OTContext> options) : base(options) { }

    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Product> Products { get; set; }

    public DbSet<Category> Categories { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);
        modelBuilder.Entity<Employee>()
            .HasMany(e => e.Orders)
            .WithOne(o => o.Employee)
            .HasForeignKey(o => o.EmployeeId);
        modelBuilder.Entity<Customer>()
            .HasMany(c => c.Orders)
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.CustomerId);

        modelBuilder.Entity<Order>()
            .HasMany(o => o.orderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId);
        modelBuilder.Entity<Product>() 
            .HasMany(p => p.orderItems)
            .WithOne(oi => oi.Product)
            .HasForeignKey(oi => oi.ProductId);
        modelBuilder.Entity<OrderItem>()
            .HasKey(oi => new { oi.OrderId, oi.ProductId });


        

    }



}
