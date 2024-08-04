using EFDataAccessLibrary.Models;
using System.ComponentModel.DataAnnotations;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public ProductType Type { get; set; }
    public string PicturePath { get; set; }


    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public ICollection<Order> Order { get; set; }
}
public enum ProductType
{
    Service,
    Product,
}