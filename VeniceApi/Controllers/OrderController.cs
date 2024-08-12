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
            var savedOrder =await _repositoryManager.Order.Add(order);
            _repositoryManager.Save();
            var orderItemList = new List<OrderItem>();

            foreach (var item in orderDto.orderItems)
            {
                
                var orderItem = mapper.Map<OrderItem>(item);
                orderItemList.Add(new OrderItem { OrderId = savedOrder.Id ,ProductId = item.ProductId});
            }
                await _repositoryManager.OrderItem.AddRange(orderItemList);

            //var orderItemDto = new OrderItemDto()
            //{
            //    OrderId = confirm.Id,
            //    ProductId = productIdkey.Id
            //};
            //var orderItem = mapper.Map<OrderItem>(orderItemDto);

            // Save the new order item to the database
            //await _repositoryManager.OrderItem.Add(orderItem);
            _repositoryManager.Save();

            return orderDto;
        }
    }
}
