using Library.Application.UseCases.Books.Queries.GetBooksList;
using Library.Application.Utilities.Mediator;

namespace Library.Application.UseCases.Books.Queries.GetBooksByCategory
{
    public class GetBooksByCategoryQuery : IRequest<IEnumerable<BookListItemDTO>>
    {
        public Guid CategoryId { get; set; }
    }
}
