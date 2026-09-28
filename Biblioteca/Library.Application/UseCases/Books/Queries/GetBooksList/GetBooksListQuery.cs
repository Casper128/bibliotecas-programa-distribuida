using Library.Application.Utilities.Mediator;

namespace Library.Application.UseCases.Books.Queries.GetBooksList
{
    public class GetBooksListQuery : IRequest<IEnumerable<BookListItemDTO>>
    {
    }
}
