using Library.Domain.Entities.Books;
using Library.Domain.Exceptions;

namespace Library.Domain.Entities.Authors
{
    public sealed class Author
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public ICollection<Book> Books { get; private set; } = new List<Book>();

        private Author()
        {
        }

        public Author(string name)
        {
            ApplyNameRules(name);

            Id = Guid.CreateVersion7();
            Name = name.Trim();
        }

        private static void ApplyNameRules(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BussinesRuleException("El nombre del autor es requerido.");
            }

            if (name.Trim().Length > 128)
            {
                throw new BussinesRuleException("El nombre del autor debe tener máximo 128 caracteres.");
            }
        }
    }
}
