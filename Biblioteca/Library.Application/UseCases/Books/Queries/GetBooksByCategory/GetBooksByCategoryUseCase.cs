using MapsterMapper;
using Library.Application.Contracts.Persistence;
using Library.Application.UseCases.Books.Queries.GetBooksList;
using Library.Application.Utilities.Mediator;
using Library.Domain.Entities.Books;

namespace Library.Application.UseCases.Books.Queries.GetBooksByCategory
{
    public class GetBooksByCategoryUseCase : IRequestHandler<GetBooksByCategoryQuery, IEnumerable<BookListItemDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBooksByCategoryUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookListItemDTO>> Handle(GetBooksByCategoryQuery query)
        {
            IEnumerable<Book> books = await _unitOfWork.Books.GetByCategoryAsync(query.CategoryId);
            return _mapper.Map<List<BookListItemDTO>>(books);
        }
    }
}
