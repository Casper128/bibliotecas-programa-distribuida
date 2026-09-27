using Library.Domain.Entities.Books;

namespace Library.Application.Contracts.Repositories
{
    public interface IBooksRepository : IRepository<Book>
    {
        Task<IEnumerable<Book>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);
    }
}
