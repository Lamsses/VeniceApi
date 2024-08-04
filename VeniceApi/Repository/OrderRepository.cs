using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using VeniceApi.Interfaces;

namespace VeniceApi.Repository
{
    public class OrderRepository : BaseReopsitory<Order> , IOrderRepository
    {
        public OrderRepository(OTContext context) : base(context)
        {
            
        }
    }
}
