using Mapster;
using Library.Domain.Entities.Books;

namespace Library.Application.UseCases.Books.Queries.GetBooksList
{
    public class GetBooksListProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Book, BookListItemDTO>()
                .Map(d => d.Isbn, b => b.Isbn.Value)
                .Map(d => d.PublicationYear, b => b.PublicationYear.Value)
                .Map(d => d.AuthorName, b => b.Author.Name)
                .Map(d => d.CategoryName, b => b.Category.Name);
        }
    }
}
