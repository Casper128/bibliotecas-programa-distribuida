using Library.Application.Contracts.Repositories;
using Library.Domain.Entities.Authors;

namespace Library.Persistence.Repositories
{
    public class AuthorsRepository : Repository<Author>, IAuthorsRepository
    {
        public AuthorsRepository(DataContext context) : base(context)
        {
        }
    }
}
