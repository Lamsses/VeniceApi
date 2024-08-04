using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using VeniceApi.Interfaces;

namespace VeniceApi.Repository
{
    public class CategoryRepository : BaseReopsitory<Category>, ICategoryRepository
    {
        private readonly OTContext context;

        public CategoryRepository(OTContext context) : base(context)
        {
            this.context = context;
        }
    }

}
