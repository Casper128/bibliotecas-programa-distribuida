using Library.Application.UseCases.Books.Queries.GetBookById;
using Library.Application.UseCases.Books.Queries.GetBooksByCategory;
using Library.Application.UseCases.Books.Queries.GetBooksList;
using Library.Application.Utilities.Mediator;
using Microsoft.Extensions.DependencyInjection;
using Mapster;
using MapsterMapper;

namespace Library.Application
{
    public static class ApplicationServicesRegistry
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            TypeAdapterConfig config = new();
            config.Scan(typeof(ApplicationServicesRegistry).Assembly);
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
            services.AddScoped<IMediator, SimpleMediator>();

            services.AddScoped<IRequestHandler<GetBooksListQuery, IEnumerable<BookListItemDTO>>, GetBooksListUseCase>();
            services.AddScoped<IRequestHandler<GetBookByIdQuery, BookDetailsDTO?>, GetBookByIdUseCase>();
            services.AddScoped<IRequestHandler<GetBooksByCategoryQuery, IEnumerable<BookListItemDTO>>, GetBooksByCategoryUseCase>();
            return services;
        }
    }
}
