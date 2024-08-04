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
        public async Task<ActionResult<OrderDto>> Post(OrderDto order)
        {
            var orderMap = mapper.Map<Order>(order);
            var orderToCreate = await _repositoryManager.Order.Add(orderMap);
            var orderItems = new List<OrderItem>([
            new OrderItem {OrderId = order.Id, FixedDiscount = 1, ProductId = 11,Quantity = 1, PrecentageDiscount = 1}

            ]);
            await _repositoryManager.OrderItem.AddRange(orderItems);
            _repositoryManager.Save();
            return Ok(orderToCreate);
        }
    }
}
