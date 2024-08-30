using AutoMapper;
using EFDataAccessLibrary.Dto;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Components.Forms.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VeniceApi.Interfaces;
using VeniceApi.Repository;

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

            // Save the new order to the database
            var savedOrder = await _repositoryManager.Order.Add(order);

            // Create a list of OrderItem entities from the incoming DTO
            var orderItemList = orderDto.orderItems.Select(item => new OrderItem
            {
                OrderId = savedOrder.Id, // Set the foreign key for each OrderItem
                ProductId = item.ProductId,
                // Add other properties as needed
            }).ToList();

            // Add the list of OrderItem entities to the database
            var response = await _repositoryManager.OrderItem.AddRange(orderItemList);
            await _repositoryManager.Save(); // Save changes asynchronously

            return Ok(orderDto); // Return the DTO if needed
        }


    }
}
