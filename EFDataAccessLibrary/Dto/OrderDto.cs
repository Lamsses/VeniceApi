using EFDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Dto
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Recipt { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }

        public List<OrderItemDto> orderItems { get; set; }




    }
}
