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
            .HasMany(o => o.Products)
            .WithMany(p => p.Orders)
            .UsingEntity<OrderItem>();

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "مسبح" },
            new Category { Id = 2, Name = "جم نسائي" },
            new Category { Id = 3, Name = "علاج فيزيائي" },
            new Category { Id = 4, Name = " دورات سباحة" },
            new Category { Id = 5, Name = "مبيعات" },
            new Category { Id = 6, Name = "غير ذلك" }
            );

       


    }



}
