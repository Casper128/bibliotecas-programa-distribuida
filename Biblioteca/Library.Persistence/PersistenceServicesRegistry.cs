using Library.Application.Contracts.Persistence;
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

            // Los repositorios específicos y los IDataSeeder se registrarán junto con el modelo.
            return services;
        }
    }
}
