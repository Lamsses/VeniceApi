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
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public EmployeeController(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> Get()
        {
            var employees = await _repositoryManager.Employee.GetAll();
            return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(employees));
        }
        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> Post(EmployeeDto employeeDto)
        {
            var employee = await _repositoryManager.Employee.Add(_mapper.Map<Employee>(employeeDto));
            return CreatedAtAction("Get", new { id = employee.Id }, _mapper.Map<EmployeeDto>(employee));
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeDto>> Put(int id, [FromBody] EmployeeDto employeeDto)
        {
            var employee = await _repositoryManager.Employee.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }

            _mapper.Map(employeeDto, employee);
            await _repositoryManager.Employee.Update(employee);

            return Ok(employeeDto);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmployeeDto>> Delete(int id)
        {
            var employee = await _repositoryManager.Employee.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }

            await _repositoryManager.Employee.Delete(id);

            return Ok(employee);
        }
    }
}
