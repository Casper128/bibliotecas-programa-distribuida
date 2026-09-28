namespace Library.Application.UseCases.Books.Queries.GetBooksList
{
    public class BookListItemDTO
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public string Isbn { get; init; } = null!;
        public int PublicationYear { get; init; }
        public Guid AuthorId { get; init; }
        public string AuthorName { get; init; } = null!;
        public Guid CategoryId { get; init; }
        public string CategoryName { get; init; } = null!;
    }
}
