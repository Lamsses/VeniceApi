using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using VeniceApi.Interfaces;

namespace VeniceApi.Repository
{
    public class ExpenseRepository : BaseReopsitory<Expense>, IExpenseRepository
    {
        public ExpenseRepository(OTContext context) : base(context)
        {
        }
    }
}
