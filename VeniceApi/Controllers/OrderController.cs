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
            var orders = await _repositoryManager.Order.GetAllQuery()
                .Include(o => o.Customer)
                .ToListAsync();

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
                CustomerId = o.CustomerId ?? null,
                CustomerName = o.Customer != null ? o.Customer.Name : "empty", // Handle null Customer
                EmployeeId = o.EmployeeId ?? null,
                OrderItems = orderItems.Where(oi => oi.OrderId == o.Id).ToList()
            }).ToList();

            return Ok(ordersDto);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> Get(Guid id)
        {
            var order = await _repositoryManager.Order.FindByCondition(o => o.Id == id, false)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            var orderItems = await _repositoryManager.OrderItem.FindByCondition(oi => oi.OrderId == id, false).ToListAsync();

            var orderDto = new OrderDto()
            {
                Id = order.Id,
                Recipt = order.Recipt,
                OrderDate = order.OrderDate,
                Status = order.Status,
                FixedDiscount = order.FixedDiscount,
                PercentageDiscount = order.PercentageDiscount,
                TotalAmount = order.TotalAmount,
                CustomerId = order.CustomerId ?? null,
                CustomerName = order.Customer != null ? order.Customer.Name : "empty", // Handle null Customer
                EmployeeId = order.EmployeeId ?? null,
                OrderItems = orderItems
            };

            return Ok(orderDto);
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
        [HttpPut("updateStatus/{id}")]
        public async Task<ActionResult<OrderDto>> Put(Guid id, [FromBody] OrderDto orderDto)
        {
            var order = await _repositoryManager.Order.FindByCondition(o => o.Id == id, false)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            mapper.Map(orderDto, order);
            await _repositoryManager.Order.Update(order);
            await _repositoryManager.Save();

            return Ok(orderDto);
        }

    }
}
