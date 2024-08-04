
namespace VeniceApi.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
     Task<Product> GetByIdWithNoTracking(int id);
    }


}
