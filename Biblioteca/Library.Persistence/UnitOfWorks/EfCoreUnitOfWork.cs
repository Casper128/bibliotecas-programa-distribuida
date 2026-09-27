using Library.Application.Contracts.Persistence;

namespace Library.Persistence.UnitOfWorks
{
    public class EfCoreUnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public EfCoreUnitOfWork(DataContext context)
        {
            _context = context;
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
