using Mapster;
using Library.Domain.Entities.Books;

namespace Library.Application.UseCases.Books.Queries.GetBookById
{
    public class GetBookByIdProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Book, BookDetailsDTO>()
                .Map(d => d.Isbn, b => b.Isbn.Value)
                .Map(d => d.PublicationYear, b => b.PublicationYear.Value)
                .Map(d => d.AuthorName, b => b.Author.Name)
                .Map(d => d.CategoryName, b => b.Category.Name);
        }
    }
}
