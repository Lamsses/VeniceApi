using System.Collections;
using System.Runtime.InteropServices.JavaScript;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VeniceApi.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VeniceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;

        public ReportController(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        [HttpPost("Order")]
        public async Task<IActionResult> OrderReports(
            [FromBody] OrderReportRequest request,
            DateTime fromDate,
            DateTime toDate
            )

        {
            var orders = await _repositoryManager.Order.
                FindByCondition(o => (o.OrderDate >= fromDate && o.OrderDate <= toDate)
                    , false ).ToListAsync();
            if (request.OrderStatuses != null && request.OrderStatuses.Any())
            {
                orders = orders.Where(o => request.OrderStatuses.Contains((int)o.Status)).ToList();
            }

            //var orderItems = new List<OrderItem>();
            var orderReportList = new List<OrderReport>();
            foreach (var order in orders)
            {
                var  orderItems =  await _repositoryManager.OrderItem
                    .FindByCondition(oi => oi.OrderId == order.Id, false).ToListAsync();
                foreach (var item in orderItems)
                {
                    var product = await _repositoryManager.Product
                        .FindByCondition(p => p.Id == item.ProductId && (request.CategoryIds == null || request.CategoryIds.Contains(p.CategoryId)), false).SingleOrDefaultAsync();
                    var category = await _repositoryManager.Category.FindByCondition(c => c.Id == product.CategoryId, false).SingleOrDefaultAsync()
                        ;
                    var customer =  await _repositoryManager.Customer.FindByCondition(c => c.Id == order.CustomerId, false).SingleOrDefaultAsync();
                    orderReportList.Add(new OrderReport
                    {
                        Recipte = order.Recipt,
                        ProductName = product!.Name,
                        CategoryName = category!.Name,
                        CustomerName = customer!.Name,
                        OrderStatus = order.Status,
                        ProductPrice = product.Price
                        
                    });
                }

            }

            var orderAnalytic = new OrderAnalytic()
            {
                OrdersCount = orders.Count(),
                TotalOrdersProfit = orders.Sum(o => o.TotalAmount),
            };


            return Ok( new
            {
                OrderReports = orderReportList,
                OrderAnalytic = orderAnalytic
            });
        }

        [HttpGet("Expense")]
        public async Task<IActionResult> ExpensesReport(DateTime fromDate, DateTime toDate)
        {
            var expenses = await _repositoryManager.Expense
                .FindByCondition(e => e.CreatedDate >= fromDate && e.CreatedDate <= toDate, false).ToListAsync();
            var expensesReport = expenses.Select(e => new ExpenseReport()
            {
                ExpenseDescription = e.Description,
                ExpenseName = e.Name,
                ExpensePrice = e.Cost,
            });
            var expenseAnalytic = new ExpenseAnalytic()
            {
                TotalExpenses = expenses.Count(),
                TotalExpensesSpent = expenses.Sum(e => e.Cost)

            };
            return Ok(new
            {
                ExpensesReport = expensesReport,
                ExpensesAnalytic = expenseAnalytic
            });
        }
    }
}
