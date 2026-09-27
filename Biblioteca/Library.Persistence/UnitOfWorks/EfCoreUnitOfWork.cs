using Library.Application.Contracts.Persistence;
using Library.Application.Contracts.Repositories;

namespace Library.Persistence.UnitOfWorks
{
    public class EfCoreUnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public IBooksRepository Books { get; }
        public IAuthorsRepository Authors { get; }
        public ICategoriesRepository Categories { get; }

        public EfCoreUnitOfWork(DataContext context,
                               IBooksRepository books,
                               IAuthorsRepository authors,
                               ICategoriesRepository categories)
        {
            _context = context;
            Books = books;
            Authors = authors;
            Categories = categories;
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Task RollbackAsync()
        {
            // Descarta cambios pendientes; no revierte un CommitAsync ya persistido.
            _context.ChangeTracker.Clear();
            return Task.CompletedTask;
        }
    }
}
