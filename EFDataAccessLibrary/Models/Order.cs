
namespace EFDataAccessLibrary.Models;

public class Order
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string Recipt { get; set; }
    public decimal FixedDiscount { get; set; } = 0;
    public int PercentageDiscount { get; set; } = 0;
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } 

    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }

    public ICollection<Product> Products { get; set; }
}

public enum OrderStatus
{
    Completed,
    Refunded,
}
