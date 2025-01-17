﻿using AutoMapper;
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
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseController(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> Get()
        {
            var expenses = await _expenseRepository.GetAll();
            return Ok(expenses);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> Get(int id)
        {
            var expense = await _expenseRepository.GetById(id);
            if (expense == null)
            {
                return NotFound();
            }
            return Ok(expense);
        }
        [HttpPost]
        public async Task<ActionResult<Expense>> Post(Expense expense)
        {
            var newExpense = await _expenseRepository.Add(expense);
            return CreatedAtAction("Get", new { id = newExpense.Id }, newExpense);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Expense>> Put(int id, [FromBody] Expense expense)
        {
            var expenseToUpdate = await _expenseRepository.GetById(id);
            if (expenseToUpdate == null)
            {
                return NotFound();
            }

            await _expenseRepository.Update(expense);

            return Ok(expense);
        }
    }
}
