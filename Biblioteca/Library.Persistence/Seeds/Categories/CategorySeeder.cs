using Library.Domain.Entities.Categories;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence.Seeds.Categories
{
    public class CategorySeeder : IDataSeeder
    {
        private readonly DataContext _context;

        public CategorySeeder(DataContext context)
        {
            _context = context;
        }

        public int Order => 2;

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            string[] names = { "Novela", "Tecnología", "Ciencia" };

            foreach (string name in names)
            {
                if (await _context.Categories.AnyAsync(c => c.Name == name, cancellationToken))
                {
                    continue;
                }

                await _context.Categories.AddAsync(new Category(name), cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
