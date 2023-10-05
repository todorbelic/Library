using BookStoreAPI.DTO.Book;
using BookStoreAPI.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "LIBRARIAN, ADMINISTRATOR")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            this._bookService = bookService;
        }

        /// <summary>
        /// Authorized for: ADMINISTRATOR, LIBRARIAN, CUSTOMER
        /// </summary>
        /// <response code="200">Request successful, all books retrieved</response>
        /// <response code="401">User cannot be authenticated</response>
        /// <response code="400">Books couldn't be retrieved</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpGet("getAll")]
        public IActionResult GetAll()
        {
            return Ok(_bookService.GetAll());
        }

        /// <summary>
        /// Authorized for: ADMINISTRATOR, LIBRARIAN, CUSTOMER
        /// </summary>
        /// <response code="200">Request successful, book retrieved</response>
        /// <response code="401">User cannot be authenticated</response>
        /// <response code="400">Book couldn't be retrieved</response>
        ///
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _bookService.GetById(id));
        }

        /// <summary>
        /// Authorized for: ADMINISTRATOR, LIBRARIAN
        /// </summary>
        /// <response code="200">Request successful, book created</response>
        /// <response code="401">User cannot be authenticated</response>
        /// <response code="403">User does not have the authorization for this request</response>
        /// <response code="400">Book couldn't be created</response>
        ///
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateBookDTO bookDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            await _bookService.Create(bookDTO);
            return Ok("Book successfully created");
        }

        /// <summary>
        /// Authorized for: ADMINISTRATOR, LIBRARIAN
        /// </summary>
        /// <response code="200">Request successful, book updated</response>
        /// <response code="401">User cannot be authenticated</response>
        /// <response code="403">User does not have the authorization for this request</response>
        /// <response code="400">Book couldn't be updated</response>
        ///
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] BookDTO bookDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _bookService.Update(bookDTO);
            return Ok("Book successfully updated");
        }

        /// <summary>
        /// Authorized for: ADMINISTRATOR, LIBRARIAN
        /// </summary>
        /// <response code="200">Request successful, author added to book</response>
        /// <response code="401">User cannot be authenticated</response>
        /// <response code="403">User does not have the authorization for this request</response>
        /// <response code="400">Author couldn't be added to book</response>
        ///
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPut("addAuthor")]
        public async Task<IActionResult> AddAuthorById([FromBody] BookAuthorDTO bookAuthorDTO)
        {
            await _bookService.AddAuthorById(bookAuthorDTO);
            return Ok("Author successfully added to book");
        }


        /// <summary>
        /// Authorized for: ADMINISTRATOR, LIBRARIAN
        /// </summary>
        /// <response code="200">Request successful, author removed from book</response>
        /// <response code="401">User cannot be authenticated</response>
        /// <response code="403">User does not have the authorization for this request</response>
        /// <response code="400">Author couldn't be removed from book</response>
        ///
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPut("removeAuthor")]
        public async Task<IActionResult> RemoveAuthorById([FromBody] BookAuthorDTO bookAuthorDTO)
        {
            await _bookService.RemoveAuthorById(bookAuthorDTO);
            return Ok("Author successfully removed from book");
        }

        /// <summary>
        /// Authorized for: ADMINISTRATOR, LIBRARIAN
        /// </summary>
        /// <response code="200">Request successful, book deleted</response>
        /// <response code="401">User cannot be authenticated</response>
        /// <response code="403">User does not have the authorization for this request</response>
        /// <response code="400">Book couldn't be deleted</response>
        ///
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            await _bookService.DeleteById(id);
            return Ok("Book successfully deleted");
        }
    }
}
