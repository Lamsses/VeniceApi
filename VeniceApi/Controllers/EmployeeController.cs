using AutoMapper;
using EFDataAccessLibrary.Dto;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VeniceApi.Interfaces;

namespace VeniceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeRepository employeeRepository ,IMapper mapper) 
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> Get()
        {
            var employees = await _employeeRepository.GetAll();
            return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(employees));
        }
        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> Post(EmployeeDto employeeDto)
        {
            var employee = await _employeeRepository.Add(_mapper.Map<Employee>(employeeDto));
            return CreatedAtAction("Get", new { id = employee.Id }, _mapper.Map<EmployeeDto>(employee));
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeDto>> Put(int id, [FromBody] EmployeeDto employeeDto)
        {
            var employee = await _employeeRepository.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }

            _mapper.Map(employeeDto, employee);
            await _employeeRepository.Update(employee);

            return Ok(employeeDto);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmployeeDto>> Delete(int id)
        {
            var employee = await _employeeRepository.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }

            await _employeeRepository.Delete(id);

            return Ok(employee);
        }
    }
}
