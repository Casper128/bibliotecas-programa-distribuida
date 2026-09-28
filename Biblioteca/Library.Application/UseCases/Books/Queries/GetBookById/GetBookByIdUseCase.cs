using MapsterMapper;
using Library.Application.Contracts.Persistence;
using Library.Application.Utilities.Mediator;
using Library.Domain.Entities.Books;

namespace Library.Application.UseCases.Books.Queries.GetBookById
{
    public class GetBookByIdUseCase : IRequestHandler<GetBookByIdQuery, BookDetailsDTO?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBookByIdUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BookDetailsDTO?> Handle(GetBookByIdQuery query)
        {
            Book? book = await _unitOfWork.Books.GetByIdAsync(query.Id);
            return book is null ? null : _mapper.Map<BookDetailsDTO>(book);
        }
    }
}
