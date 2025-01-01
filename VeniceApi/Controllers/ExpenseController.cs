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
    public class ExpenseController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public ExpenseController(IRepositoryManager repositoryManager, IMapper mapper)
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
        public async Task<ActionResult> GetExpenses(int page = 1, int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page and pageSize must be greater than 0.");
            }

            // Get all expenses
            var expenses = await _repositoryManager.Expense.GetAll();

            // Filter visible expenses
            var visibleExpenses = expenses.Where(e => e.IsVisible).OrderByDescending(e => e.CreatedDate);

            // Calculate total number of expenses and pages
            var totalExpenses = visibleExpenses.Count();
            var totalPages = (int)Math.Ceiling((double)totalExpenses / pageSize);

            // Apply pagination
            var paginatedExpenses = visibleExpenses
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();  // Materialize the query to avoid deferred execution

            // Return the paginated result, total pages, and total items count
            var result = new
            {
                TotalPages = totalPages,
                TotalItems = totalExpenses,  // Optionally, return total number of items
                Expenses = paginatedExpenses
            };

            return Ok(result);
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
            expense.UpdatedDate = DateTime.Now;
            expense.CreatedDate = DateTime.Now;
            expense.RandomId = GenerateRandomId();
            await _repositoryManager.Save();
            return CreatedAtAction("Get", new { id = newExpense.Id }, newExpense);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<ExpenseDto>> Put(int id, [FromBody] ExpenseDto expense)
        {
            var expenseToUpdate = await _repositoryManager.Expense.GetById(id);
            if (expenseToUpdate == null)
            {
                return NotFound();
            }

            var newExpense =_mapper.Map(expense, expenseToUpdate);
            await _repositoryManager.Expense.Update(newExpense);
            await _repositoryManager.Save();
            return Ok(expense);
        }
        
    }
}
