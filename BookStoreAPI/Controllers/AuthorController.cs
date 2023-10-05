using BookStoreAPI.DTO.Author;
using BookStoreAPI.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "LIBRARIAN, ADMINISTRATOR")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            this._authorService = authorService;
        }

        /// <summary>
        /// Authorized for: ADMINISTRATOR, LIBRARIAN, CUSTOMER
        /// </summary>
        /// <response code="200">Request successful, all authors retrieved</response>
        /// <response code="401">User cannot be authenticated</response>
        /// <response code="403">User does not have the authorization for this request</response>
        /// <response code="400">Authors couldn't be retrieved</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "LIBRARIAN, ADMINISTRATOR, CUSTOMER")]
        [HttpGet("getAll")]
        public IActionResult GetAll()
        {
            return Ok(_authorService.GetAll());
        }

        /// <summary>
        /// Authorized for: ADMINISTRATOR, LIBRARIAN, CUSTOMER
        /// </summary>
        /// <response code="200">Request successful, author's books retrieved</response>
        /// <response code="401">User cannot be authenticated</response>
        /// <response code="400">Author's books couldn't be retrieved</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "LIBRARIAN, ADMINISTRATOR, CUSTOMER")]
        [HttpGet("getAllBooksByAuthor/{id}")]
        public async Task<IActionResult> GetAllBooks(Guid id)
        {
            return Ok(await _authorService.GetAllBooksByAuthorId(id));
        }

        /// <summary>
        /// Authorized for: ADMINISTRATOR, LIBRARIAN, CUSTOMER
        /// </summary>
        /// <response code="200">Request successful, author is retrieved</response>
        /// <response code="401">User cannot be authenticated</response>
        /// <response code="400">Author couldn't be retrieved</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "LIBRARIAN, ADMINISTRATOR, CUSTOMER")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _authorService.GetById(id));
        }

        /// <summary>
        /// Authorized for: ADMINISTRATOR, LIBRARIAN
        /// </summary>
        /// <response code="200">Request successful, author is created</response>
        /// <response code="401">User cannot be authenticated</response>
        /// <response code="403">User does not have the authorization for this request</response>
        /// <response code="400">Author couldn't be created</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateAuthorDTO authorDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            await _authorService.Create(authorDTO);
            return Ok("Author successfully created");
        }

        /// <summary>
        /// Authorized for: ADMINISTRATOR, LIBRARIAN
        /// </summary>
        /// <response code="200">Request successful, author is updated</response>
        /// <response code="401">User cannot be authenticated</response>
        /// <response code="403">User does not have the authorization for this request</response>
        /// <response code="400">Author couldn't be updated</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] AuthorDTO authorDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _authorService.Update(authorDTO);
            return Ok("Author successfully updated");
        }

        /// <summary>
        /// Authorized for: ADMINISTRATOR, LIBRARIAN
        /// </summary>
        /// <response code="200">Request successful, author is deleted</response>
        /// <response code="401">User cannot be authenticated</response>
        /// <response code="403">User does not have the authorization for this request</response>
        /// <response code="400">Author couldn't be deleted</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            await _authorService.DeleteById(id);
            return Ok("Author successfully deleted");
        }
    }
}
