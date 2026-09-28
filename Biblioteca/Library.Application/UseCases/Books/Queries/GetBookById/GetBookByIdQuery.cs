using Library.Application.Utilities.Mediator;

namespace Library.Application.UseCases.Books.Queries.GetBookById
{
    public class GetBookByIdQuery : IRequest<BookDetailsDTO?>
    {
        public Guid Id { get; set; }
    }
}
