using Library.Application.Contracts.Repositories;

namespace Library.Application.Contracts.Persistence
{
    public interface IUnitOfWork
    {
        IBooksRepository Books { get; }
        IAuthorsRepository Authors { get; }
        ICategoriesRepository Categories { get; }

        Task CommitAsync();
        Task RollbackAsync();
    }
}
