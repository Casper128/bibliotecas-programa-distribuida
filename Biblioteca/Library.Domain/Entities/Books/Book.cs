using Library.Domain.Entities.Authors;
using Library.Domain.Entities.Books.ValueObjects;
using Library.Domain.Entities.Categories;
using Library.Domain.Exceptions;

namespace Library.Domain.Entities.Books
{
    public sealed class Book
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; } = null!;
        public Isbn Isbn { get; private set; } = null!;
        public PublicationYear PublicationYear { get; private set; } = null!;
        public Guid AuthorId { get; private set; }
        public Author Author { get; private set; } = null!;
        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; } = null!;

        private Book()
        {
        }

        public Book(string title, Isbn isbn, PublicationYear publicationYear, Guid authorId, Guid categoryId)
        {
            ApplyTitleRules(title);
            ApplyIsbnRules(isbn);
            ApplyPublicationYearRules(publicationYear);
            ApplyAuthorIdRules(authorId);
            ApplyCategoryIdRules(categoryId);

            Id = Guid.CreateVersion7();
            Title = title.Trim();
            Isbn = isbn;
            PublicationYear = publicationYear;
            AuthorId = authorId;
            CategoryId = categoryId;
        }

        private static void ApplyTitleRules(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new BussinesRuleException("El título del libro es requerido.");
            }

            if (title.Trim().Length > 256)
            {
                throw new BussinesRuleException("El título del libro debe tener máximo 256 caracteres.");
            }
        }

        private static void ApplyIsbnRules(Isbn isbn)
        {
            if (isbn is null)
            {
                throw new BussinesRuleException("El ISBN del libro es requerido.");
            }
        }

        private static void ApplyPublicationYearRules(PublicationYear publicationYear)
        {
            if (publicationYear is null)
            {
                throw new BussinesRuleException("El año de publicación del libro es requerido.");
            }
        }

        private static void ApplyAuthorIdRules(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                throw new BussinesRuleException("El autor del libro es requerido.");
            }
        }

        private static void ApplyCategoryIdRules(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
            {
                throw new BussinesRuleException("La categoría del libro es requerida.");
            }
        }
    }
}
