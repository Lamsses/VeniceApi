using AutoMapper;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Dto;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VeniceApi.Interfaces;

namespace VeniceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper mapper;

        public OrderController(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            this.mapper = mapper;
        }
        public static int GenerateRandomId()
        {
            Random random = new Random();
            return random.Next(100000, 1000000); // Generates a number between 100000 and 999999
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> Get()
        {
            var orders = await _repositoryManager.Order.GetAllQuery().Include(o => o.Customer).ToListAsync();
            var orderItems = await _repositoryManager.OrderItem.GetAll();
            var ordersDto = orders.Select(o => new OrderDto()
            {
                Id = o.Id,
                Recipt = o.Recipt,
                OrderDate = o.OrderDate,
                Status = o.Status,
                FixedDiscount = o.FixedDiscount,
                PercentageDiscount = o.PercentageDiscount,
                TotalAmount = o.TotalAmount,
                CustomerId = o.CustomerId,
                CustomerName = o.Customer.Name,
                EmployeeId = o.EmployeeId,
                OrderItems = orderItems.Where(oi => oi.OrderId == o.Id).ToList()
            }).ToList();
           
            return Ok(ordersDto);
        }

        [HttpGet("orderItems/{orderId}")]

        public async Task<ActionResult<IEnumerable<Product>>> GetOrderItem(Guid orderId)
        {
            var orderItems = await _repositoryManager.OrderItem.FindByCondition(e => e.OrderId == orderId , false).FirstOrDefaultAsync();


            var products = await _repositoryManager.Product.FindByCondition
                (e => e.Id == orderItems!.ProductId, false).ToListAsync();

                   
            return Ok(products);
        }
        [HttpPost]
        public async Task<ActionResult<OrderDto>> Post([FromBody] OrderDto orderDto)
        {
            var order = mapper.Map<Order>(orderDto);
            order.Id = Guid.NewGuid();

            order.Recipt = GenerateRandomId().ToString();
            await _repositoryManager.Order.Add(order);
            // Assign foreign key for each OrderItem
            var orderItemsList = orderDto.OrderItems.Select(i => new OrderItem()
            {
                ProductId = i.ProductId,
                OrderId = order.Id,
                Quantity = i.Quantity
            }).ToList();

            foreach (var items in orderItemsList)
            {
                var product = await _repositoryManager.Product.GetById(items.ProductId);
                product.InStock -= items.Quantity;
                
            }

            await _repositoryManager.OrderItem.AddRange(orderItemsList);
            // Add order and its related OrderItems at once (EF will handle the tracking)
            await _repositoryManager.Save(); // Save changes asynchronously

            return Ok(orderDto); // Return the DTO if needed
        }
    }
}
