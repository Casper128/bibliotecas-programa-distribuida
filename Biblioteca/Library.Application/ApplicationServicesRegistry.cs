using Library.Application.Utilities.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Application
{
    public static class ApplicationServicesRegistry
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(configuration => { }, typeof(ApplicationServicesRegistry).Assembly);
            services.AddScoped<IMediator, SimpleMediator>();

            // Los casos de uso de consulta se registrarán en su respectiva etapa.
            return services;
        }
    }
}
