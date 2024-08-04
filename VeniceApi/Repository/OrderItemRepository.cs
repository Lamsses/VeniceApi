using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using VeniceApi.Interfaces;

namespace VeniceApi.Repository
{
    public class OrderItemRepository : BaseReopsitory<OrderItem> , IOrderItemRepository
    {
        public OrderItemRepository(OTContext context) : base(context)
        {
            
        }

    }
}
