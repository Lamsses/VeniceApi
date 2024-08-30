using AutoMapper;
using EFDataAccessLibrary.Dto;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VeniceApi.Interfaces;
using VeniceApi.Repository;

namespace VeniceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public CustomerController(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> Get()
        {
            var customers = await _repositoryManager.Customer.GetAll();
            return Ok(_mapper.Map<IEnumerable<CustomerDto>>(customers));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> Get(int id)
        {
            var product = await _repositoryManager.Customer.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Post(CustomerDto customerDto)
        {
            var customer = await _repositoryManager.Customer.Add(_mapper.Map<Customer>(customerDto));
            await _repositoryManager.Save();
            return CreatedAtAction("Get", new { id = customer.Id }, _mapper.Map<CustomerDto>(customer));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerDto>> Put(int id, [FromBody] CustomerDto customerDto)
        {
            var customer = await _repositoryManager.Customer.GetById(id);
            if (customer == null)
            {
                return NotFound();
            }

            _mapper.Map(customerDto, customer);
            await _repositoryManager.Customer.Update(customer);
            await _repositoryManager.Save();

            return Ok(customerDto);
        }

    }
}
