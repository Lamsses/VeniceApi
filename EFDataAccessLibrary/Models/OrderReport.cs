namespace EFDataAccessLibrary.Models
{
    public class OrderReport
    {
        public string Recipte { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string CustomerName { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public decimal ProductPrice { get; set; }



    }

}
