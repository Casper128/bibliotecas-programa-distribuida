using Library.Application.UseCases.Books.Queries.GetBookById;
using Library.Application.UseCases.Books.Queries.GetBooksByCategory;
using Library.Application.UseCases.Books.Queries.GetBooksList;
using Library.Application.Utilities.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BooksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookListItemDTO>>> GetList()
        {
            IEnumerable<BookListItemDTO> result = await _mediator.Send(new GetBooksListQuery());
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookDetailsDTO>> GetById(Guid id)
        {
            GetBookByIdQuery query = new() { Id = id };
            BookDetailsDTO? result = await _mediator.Send(query);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("category/{categoryId:guid}")]
        public async Task<ActionResult<IEnumerable<BookListItemDTO>>> GetByCategory(Guid categoryId)
        {
            GetBooksByCategoryQuery query = new() { CategoryId = categoryId };
            IEnumerable<BookListItemDTO> result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
