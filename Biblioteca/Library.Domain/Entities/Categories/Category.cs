using Library.Domain.Entities.Books;
using Library.Domain.Exceptions;

namespace Library.Domain.Entities.Categories
{
    public sealed class Category
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public ICollection<Book> Books { get; private set; } = new List<Book>();

        private Category()
        {
        }

        public Category(string name)
        {
            ApplyNameRules(name);

            Id = Guid.CreateVersion7();
            Name = name.Trim();
        }

        private static void ApplyNameRules(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BussinesRuleException("El nombre de la categoría es requerido.");
            }

            if (name.Trim().Length > 64)
            {
                throw new BussinesRuleException("El nombre de la categoría debe tener máximo 64 caracteres.");
            }
        }
    }
}
