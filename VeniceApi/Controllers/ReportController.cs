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
        DateTime? toDate
        )
        {
            var orders = new List<Order>();

            if (toDate is null)
            {
                orders = await _repositoryManager.Order
                    .FindByCondition(o => o.OrderDate >= fromDate, false)
                    .ToListAsync();
            }
            else if (fromDate == toDate)
            {
                fromDate = fromDate.Date;
                toDate = toDate.Value.Date.AddDays(1).AddTicks(-1);
                orders = await _repositoryManager.Order
                    .FindByCondition(o => o.OrderDate >= fromDate && o.OrderDate <= toDate, false)
                    .ToListAsync();
            }
            else
            {
                toDate = toDate.Value.Date.AddDays(1).AddTicks(-1);
                orders = await _repositoryManager.Order
                    .FindByCondition(o => o.OrderDate >= fromDate && o.OrderDate <= toDate, false)
                    .ToListAsync();
            }

            if (request.OrderStatuses != null && request.OrderStatuses.Any())
            {
                orders = orders.Where(o => request.OrderStatuses.Contains((int)o.Status)).ToList();
            }

            var orderReportList = new List<OrderReport>();
            var totalOrdersProfit = 0m;
            var matchingOrdersCount = 0;

            foreach (var order in orders)
            {
                var orderItems = await _repositoryManager.OrderItem
                    .FindByCondition(oi => oi.OrderId == order.Id, false)
                    .ToListAsync();

                bool orderHasMatchingProducts = false;  // Track if the order has matching products.

                foreach (var item in orderItems)
                {
                    var product = await _repositoryManager.Product
                        .FindByCondition(p => p.Id == item.ProductId
                            && (request.CategoryIds == null || request.CategoryIds.Contains(p.CategoryId)), false)
                        .SingleOrDefaultAsync();

                    if (product is null)
                        continue;

                    orderHasMatchingProducts = true;  // Found a product from the selected categories.

                    var category = await _repositoryManager.Category
                        .FindByCondition(c => c.Id == product.CategoryId, false)
                        .SingleOrDefaultAsync();
                    var customer = await _repositoryManager.Customer
                        .FindByCondition(c => c.Id == order.CustomerId, false)
                        .SingleOrDefaultAsync();

                    orderReportList.Add(new OrderReport
                    {
                        Recipte = order.Recipt,
                        ProductName = product!.Name,
                        CategoryName = category.Name,
                        CustomerName = customer?.Name ?? "لايوجد",
                        OrderStatus = order.Status,
                        OrderDate = order.OrderDate,
                        OrderItemQuantity = item.Quantity,
                        OrderToatalAmount = order.TotalAmount,
                        ProductPrice = product.Price
                    });

                    // Calculate the total orders profit based on matching products.
                    totalOrdersProfit += product.Price * item.Quantity;
                }

                // If the order contains any product from the selected categories, count it.
                if (orderHasMatchingProducts)
                {
                    matchingOrdersCount++;
                }
            }

            var orderAnalytic = new OrderAnalytic
            {
                OrdersCount = matchingOrdersCount,  // Use the matching order count.
                TotalOrdersProfit = totalOrdersProfit, // Use the calculated profit.
            };

            return Ok(new
            {
                OrderReports = orderReportList.OrderBy(e => e.Recipte),
                OrderAnalytic = orderAnalytic
            });
        }





        [HttpGet("Expense")]
public async Task<IActionResult> ExpensesReport(DateTime fromDate, DateTime? toDate)
{
    var expenses = new List<Expense>();

    // Normalize fromDate to the start of the day
    fromDate = fromDate.Date;

    if (toDate is null)
    {
        expenses = await _repositoryManager.Expense
                    .FindByCondition(e => e.CreatedDate >= fromDate && e.IsVisible == true, false).ToListAsync();
    }
    else
    {
        // Ensure toDate includes the entire day
        toDate = toDate.Value.Date.AddDays(1).AddTicks(-1);

        expenses = await _repositoryManager.Expense
                 .FindByCondition(e => e.CreatedDate >= fromDate && e.CreatedDate <= toDate && e.IsVisible, false).ToListAsync();
    }

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
