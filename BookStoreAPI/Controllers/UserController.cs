using BookStoreAPI.DTO.User;
using BookStoreAPI.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookStoreAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAccountService _userService;

        public UserController(IAccountService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Authorized for: ADMINISTRATOR, LIBRARIAN, CUSTOMER
        /// </summary>
        /// <response code="200">Request successful, profile retrieved</response>
        /// <response code="401">User cannot be authenticated</response>
        ///<response code="400">Profile couldn't be retrieved</response>
        ///
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("getProfile")]
        public async Task<IActionResult> GetProfile()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(await _userService.GetProfile(userId));
        }

        /// <summary>
        /// Authorized for: ADMINISTRATOR, LIBRARIAN, CUSTOMER
        /// </summary>
        /// <response code="200">Request successful, avatar retrieved</response>
        /// <response code="401">User cannot be authenticated</response>
        ///<response code="400">Avatar couldn't be retrieved</response>
        ///
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("showAvatar")]
        public async Task<FileContentResult> GetAvatar()
        {
            string? id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            byte[] avatar = await _userService.GetAvatar(id);
            return File(avatar, "image/jpg");
        }

        /// <summary>
        /// Authorized for: ADMINISTRATOR, LIBRARIAN, CUSTOMER
        /// </summary>
        /// <response code="200">Request successful, avatar uploaded</response>
        /// <response code="401">User cannot be authenticated</response>
        ///<response code="400">Avatar couldn't be uploaded</response>
        ///
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("uploadAvatar")]
        public async Task<IActionResult> Upload(IFormFile avatar)
        {
            string? id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _userService.UploadAvatar(id, avatar);
            return Ok("Avatar successfully uploaded");
        }

        /// <summary>
        /// Authorized for: ADMINISTRATOR, LIBRARIAN, CUSTOMER
        /// </summary>
        /// <response code="200">Request successful, profile updated</response>
        /// <response code="401">User cannot be authenticated</response>
        ///<response code="400">Profile couldn't be updated</response>
        ///
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("updateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            string? id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _userService.UpdateUser(id, userDTO);
            return Ok("Profile successfully updated");
        }

        /// <summary>
        /// Authorized for: ADMINISTRATOR, LIBRARIAN, CUSTOMER
        /// </summary>
        /// <response code="200">Request successful, avatar removed</response>
        /// <response code="401">User cannot be authenticated</response>
        /// <response code="400">Avatar couldn't be removed</response>
        ///
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("removeAvatar")]
        public async Task<IActionResult> RemoveAvatar()
        {
            string? id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _userService.RemoveAvatar(id);
            return Ok("Avatar successfully removed");
        }
    }
}
