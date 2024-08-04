using EFDataAccessLibrary.DataAccess;
using Microsoft.EntityFrameworkCore;
using VeniceApi.Interfaces;

namespace VeniceApi.Repository
{
    public class ProductRepository : BaseReopsitory<Product>, IProductRepository
    {
        private readonly OTContext context;

        public ProductRepository(OTContext context) : base(context)
        {
            this.context = context;
        }
        public async Task<Product> GetByIdWithNoTracking(int id)
        {
            return await context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

        }

    }

}
