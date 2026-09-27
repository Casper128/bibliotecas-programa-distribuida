using Library.Domain.Entities.Authors;
using Library.Domain.Entities.Books;
using Library.Domain.Entities.Books.ValueObjects;
using Library.Domain.Entities.Categories;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence.Seeds.Books
{
    public class BookSeeder : IDataSeeder
    {
        private readonly DataContext _context;

        public BookSeeder(DataContext context)
        {
            _context = context;
        }

        public int Order => 3;

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            // Catálogo ficticio para practicar las consultas de la actividad.
            var books = new[]
            {
                (Title: "El jardín de las historias", Isbn: "9780000000019", Year: 2020, Author: "Ana Torres", Category: "Novela"),
                (Title: "La casa junto al río", Isbn: "9780000000026", Year: 2023, Author: "Ana Torres", Category: "Novela"),
                (Title: "Introducción a la programación", Isbn: "9780000000033", Year: 2022, Author: "Luis Moreno", Category: "Tecnología"),
                (Title: "Un viaje por la ciencia", Isbn: "9780000000040", Year: 2021, Author: "Clara Ríos", Category: "Ciencia")
            };

            foreach (var item in books)
            {
                if (await _context.Books.AnyAsync(b => b.Isbn.Value == item.Isbn, cancellationToken))
                {
                    continue;
                }

                Author author = await _context.Authors.FirstAsync(a => a.Name == item.Author, cancellationToken);
                Category category = await _context.Categories.FirstAsync(c => c.Name == item.Category, cancellationToken);

                Book book = new Book(item.Title,
                                     new Isbn(item.Isbn),
                                     new PublicationYear(item.Year),
                                     author.Id,
                                     category.Id);

                await _context.Books.AddAsync(book, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
