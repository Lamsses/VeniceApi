using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using VeniceApi.Interfaces;

namespace VeniceApi.Repository
{
    public class CustomerRepository : BaseReopsitory<Customer>, ICustomerRepository
    {

        public CustomerRepository(OTContext context) : base(context)
        {
        }
    }

}
