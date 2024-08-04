using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class OrderItem
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public int PrecentageDiscount { get; set; }
        public int FixedDiscount { get; set; }
        public decimal Price => FixedDiscount > 0 ? (Product.Price - FixedDiscount) : PrecentageDiscount > 0 ? Product.Price * (PrecentageDiscount/ 100 ): Product.Price; 
        public decimal TotalPrice => Price * Quantity;




    }

}
