using MapsterMapper;
using Library.Application.Contracts.Persistence;
using Library.Application.Utilities.Mediator;
using Library.Domain.Entities.Books;

namespace Library.Application.UseCases.Books.Queries.GetBooksList
{
    public class GetBooksListUseCase : IRequestHandler<GetBooksListQuery, IEnumerable<BookListItemDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBooksListUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookListItemDTO>> Handle(GetBooksListQuery query)
        {
            IEnumerable<Book> books = await _unitOfWork.Books.GetListAsync();
            return _mapper.Map<List<BookListItemDTO>>(books);
        }
    }
}
