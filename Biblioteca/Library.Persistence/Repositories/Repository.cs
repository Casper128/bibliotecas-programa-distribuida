using Library.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DataContext _context;

        public Repository(DataContext context)
        {
            _context = context;
        }

        public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<T>().AddAsync(entity, cancellationToken);
            return entity;
        }

        public Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _context.Update(entity);
            return Task.FromResult(entity);
        }

        public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            T? entity = await _context.Set<T>().FindAsync(new object[] { id }, cancellationToken);

            if (entity is not null)
            {
                _context.Remove(entity);
            }
        }

        public virtual async Task<IEnumerable<T>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
