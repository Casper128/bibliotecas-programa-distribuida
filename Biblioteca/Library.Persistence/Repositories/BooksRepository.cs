using Library.Application.Contracts.Repositories;
using Library.Domain.Entities.Books;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence.Repositories
{
    public class BooksRepository : Repository<Book>, IBooksRepository
    {
        private readonly DataContext _context;

        public BooksRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Book>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await GetQuery()
                .OrderBy(b => b.Title)
                .ThenBy(b => b.Id)
                .ToListAsync(cancellationToken);
        }

        public override async Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await GetQuery().FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Book>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default)
        {
            return await GetQuery()
                .Where(b => b.CategoryId == categoryId)
                .OrderBy(b => b.Title)
                .ThenBy(b => b.Id)
                .ToListAsync(cancellationToken);
        }

        private IQueryable<Book> GetQuery()
        {
            return _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .AsNoTracking();
        }
    }
}
