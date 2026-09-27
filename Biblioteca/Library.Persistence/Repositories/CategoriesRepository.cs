using Library.Application.Contracts.Repositories;
using Library.Domain.Entities.Categories;

namespace Library.Persistence.Repositories
{
    public class CategoriesRepository : Repository<Category>, ICategoriesRepository
    {
        public CategoriesRepository(DataContext context) : base(context)
        {
        }
    }
}
