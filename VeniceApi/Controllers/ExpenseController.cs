using AutoMapper;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VeniceApi.Interfaces;
using VeniceApi.Repository;

namespace VeniceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;

        public ExpenseController(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> Get()
        {
            var expenses = await _repositoryManager.Expense.GetAll();
            return Ok(expenses);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> Get(int id)
        {
            var expense = await _repositoryManager.Expense.GetById(id);
            if (expense == null)
            {
                return NotFound();
            }
            return Ok(expense);
        }
        [HttpPost]
        public async Task<ActionResult<Expense>> Post(Expense expense)
        {
            var newExpense = await _repositoryManager.Expense.Add(expense);
            return CreatedAtAction("Get", new { id = newExpense.Id }, newExpense);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Expense>> Put(int id, [FromBody] Expense expense)
        {
            var expenseToUpdate = await _repositoryManager.Expense.GetById(id);
            if (expenseToUpdate == null)
            {
                return NotFound();
            }

            await _repositoryManager.Expense.Update(expense);

            return Ok(expense);
        }
    }
}
