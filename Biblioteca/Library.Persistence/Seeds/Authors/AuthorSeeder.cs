using Library.Domain.Entities.Authors;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence.Seeds.Authors
{
    public class AuthorSeeder : IDataSeeder
    {
        private readonly DataContext _context;

        public AuthorSeeder(DataContext context)
        {
            _context = context;
        }

        public int Order => 1;

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            string[] names = { "Ana Torres", "Luis Moreno", "Clara Ríos" };

            foreach (string name in names)
            {
                if (await _context.Authors.AnyAsync(a => a.Name == name, cancellationToken))
                {
                    continue;
                }

                await _context.Authors.AddAsync(new Author(name), cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
