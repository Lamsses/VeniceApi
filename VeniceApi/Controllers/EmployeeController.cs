using AutoMapper;
using EFDataAccessLibrary.Dto;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using VeniceApi.Interfaces;

namespace VeniceApi.Controllers;

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
    public static int GenerateRandomId()
    {
        Random random = new Random();
        return random.Next(100000, 1000000); // Generates a number between 100000 and 999999
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> Get(int page = 1, int pageSize = 10)
    {
        var employees = await _repositoryManager.Employee.GetAll();
        var visibleEmployees = employees.Where(e => e.IsVisible); // Filter employees with visible set to true
        var totalEmployees = visibleEmployees.Count();
        var totalPages = (int)Math.Ceiling((double)totalEmployees / pageSize);

        var paginatedEmployees = visibleEmployees.Skip((page - 1) * pageSize).Take(pageSize);
        var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(paginatedEmployees);

        var result = new
        {
            TotalPages = totalPages,
            Employees = employeeDtos
        };

        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDto>> Get(int id)
    {
        var employee = await _repositoryManager.Employee.GetById(id);
        if (employee == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<EmployeeDto>(employee));
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> Post(EmployeeDto employeeDto)
    {
        employeeDto.RandomId = GenerateRandomId();
        var employee = await _repositoryManager.Employee.Add(_mapper.Map<Employee>(employeeDto));


        _repositoryManager.Save();
        return CreatedAtAction("Get", new { id = employee.Id }, _mapper.Map<EmployeeDto>(employee));
    }


    [HttpPut("{id}")]
    public async Task<ActionResult<EmployeeDto>> Put(int id, [FromBody] EmployeeDto employeeDto)
    {
        // Retrieve the existing employee entity
        var employee = await _repositoryManager.Employee.GetById(id);
        if (employee == null)
        {
            return NotFound();
        }

        // Ensure the Id in the DTO matches the entity's Id
        if (employeeDto.Id != id)
        {
            return BadRequest("The Id in the request body does not match the resource Id.");
        }

        // Map the incoming DTO to the existing employee entity
        _mapper.Map(employeeDto, employee);

        // Update the employee in the repository
        await _repositoryManager.Employee.Update(employee);

        // Save the changes
        _repositoryManager.Save();

        // Return the updated employee DTO
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
        _repositoryManager.Save();

        return Ok(employee);
    }


}
