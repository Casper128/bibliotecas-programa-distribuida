using Library.Application.Contracts.Persistence;
using Library.Persistence.Seeds;
using Library.Persistence.Seeds.Authors;
using Library.Persistence.Seeds.Books;
using Library.Persistence.Seeds.Categories;
using Library.Persistence.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Persistence
{
    public static class PersistenceServicesRegistry
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("MyConnection")
                    ?? throw new InvalidOperationException("Debe configurar ConnectionStrings:MyConnection."));
            });

            services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();

            // Seeders: primero los catálogos y después los libros relacionados.
            services.AddScoped<IDataSeeder, AuthorSeeder>();
            services.AddScoped<IDataSeeder, CategorySeeder>();
            services.AddScoped<IDataSeeder, BookSeeder>();

            return services;
        }
    }
}
