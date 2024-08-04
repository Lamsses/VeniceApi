using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using VeniceApi.Interfaces;

namespace VeniceApi.Repository
{
    public class EmployeeRepository : BaseReopsitory<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(OTContext context) : base(context)
        {
        }
    }
}
