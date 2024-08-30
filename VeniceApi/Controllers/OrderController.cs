using AutoMapper;
using EFDataAccessLibrary.Dto;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> Get()
        {
            var orders = await _repositoryManager.Order.GetAll();
            var ordersDto = mapper.Map<IEnumerable<OrderDto>>(orders);
            return Ok(ordersDto);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> Post([FromBody] OrderDto orderDto)
        {
            var order = mapper.Map<Order>(orderDto);
            order.Id = Guid.NewGuid();

            await _repositoryManager.Order.Add(order);
            // Assign foreign key for each OrderItem
            var orderItemsList = orderDto.OrderItems.Select(i => new OrderItem()
            {
                ProductId = i.ProductId,
                OrderId = order.Id,
                Quantity = i.Quantity
            }).ToList();
            await _repositoryManager.OrderItem.AddRange(orderItemsList);
            // Add order and its related OrderItems at once (EF will handle the tracking)
            await _repositoryManager.Save(); // Save changes asynchronously

            return Ok(orderDto); // Return the DTO if needed
        }
    }
}
