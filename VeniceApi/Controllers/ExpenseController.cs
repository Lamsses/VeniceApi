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

        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses(int page = 1, int pageSize = 10)
        {
            var expenses = await _repositoryManager.Expense.GetAll();
            var visibleExpenses = expenses.Where(e => e.IsVisible); // Filter expenses with visible set to true
            var totalExpenses = visibleExpenses.Count();
            var totalPages = (int)Math.Ceiling((double)totalExpenses / pageSize);

            var paginatedExpenses = visibleExpenses.Skip((page - 1) * pageSize).Take(pageSize);

            var result = new
            {
                TotalPages = totalPages,
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
