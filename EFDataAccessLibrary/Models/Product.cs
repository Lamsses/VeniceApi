using EFDataAccessLibrary.Models;
using System.ComponentModel.DataAnnotations;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime UpdatedDate { get; set; } = DateTime.Now;
    public ProductType Type { get; set; }
    public string PicturePath { get; set; }
    
    public bool IsVisible { get; set; } = true;

    public int RandomId { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public ICollection<OrderItem> orderItems { get; set; }
}
public enum ProductType
{
    Service,
    Product,
}