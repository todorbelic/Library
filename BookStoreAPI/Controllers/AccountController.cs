using BookStoreAPI.DTO.User;
using BookStoreAPI.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _userService;
        private readonly IUserAuthenticationService _userAuthenticationService;

        public AccountController(IAccountService userService, IUserAuthenticationService userAuthenticationService)
        {
            _userService = userService;
            _userAuthenticationService = userAuthenticationService;
        }

        /// <summary>
        /// Authorized for: ANYONE
        /// </summary>
        /// <response code="200">Request successful, user registered</response>
        /// <response code="401">User cannot be authenticated</response>
        /// <response code="400">User couldn't be registered</response>
        ///
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost("registerUser")]
        public async Task<IActionResult> RegisterUser(UserRegistrationDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _userService.RegisterUserAsync(user);
                return !result.Succeeded ? new BadRequestObjectResult(result) : StatusCode(201);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Authorized for: ADMINISTRATOR
        /// </summary>
        /// <response code="200">Request successful, user registered</response>
        /// <response code="401">User cannot be authenticated</response>
        /// <response code="400">User couldn't be registered</response>
        /// <response code="403">User does not have the authorization for this request</response>
        ///
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpPost("registerLibrarian")]
        public async Task<IActionResult> RegisterLibrarian(LibrarianRegistrationDTO librarian)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _userService.RegisterUserAsync(librarian);
                return !result.Succeeded ? new BadRequestObjectResult(result) : StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Authorized for: ANYONE
        /// </summary>
        /// <response code="200">Request successful, user logged in</response>
        /// <response code="401">User cannot be authenticated</response>
        /// <response code="400">User couldn't be logged in</response>
        ///
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            string? ipAdress = HttpContext.Connection.RemoteIpAddress?.ToString();
            string userAgent = Request.Headers["User-Agent"].ToString();
            return Ok(await _userAuthenticationService.ValidateUserLogin(userLogin, ipAdress, userAgent));
        }

        /// <summary>
        /// Authorized for: ANYONE
        /// </summary>
        /// <response code="200">Request successful, generated new refresh and jwt token</response>
        /// <response code="401">Invalid tokens provided</response>
        /// <response code="400">Token couldn't be refreshed</response>
        ///
        [AllowAnonymous]
        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO refreshTokenRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            string? ipAdress = HttpContext.Connection.RemoteIpAddress?.ToString();
            string userAgent = Request.Headers["User-Agent"].ToString();

            return Ok(await _userAuthenticationService.RefreshToken(refreshTokenRequest, ipAdress, userAgent));
        }


        /// <summary>
        /// Authorized for: ADMINISTRATOR
        /// </summary>
        /// <response code="200">Request successful, generated new refresh and jwt token</response>
        /// <response code="401">Invalid tokens provided</response>
        /// <response code="400">Token couldn't be refreshed</response>
        ///
        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpPatch("revokeAllTokens")]
        public async Task<IActionResult> RevokeAllTokens()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            string? ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            await _userAuthenticationService.RevokeAllRefreshTokens(ipAddress);
            return Ok();
        }

        /// <summary>
        /// Authorized for: ADMINISTRATOR
        /// </summary>
        /// <response code="200">Request successful, generated new refresh and jwt token</response>
        /// <response code="401">Invalid tokens provided</response>
        /// <response code="400">Token couldn't be refreshed</response>
        ///
        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpPatch("revokeAllTokensForUser")]
        public async Task<IActionResult> RevokeAllTokensForUser([FromBody] RevokeUserTokenRequest revokeUserTokenRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            string? ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            await _userAuthenticationService.RevokeAllRefreshTokensForUser(revokeUserTokenRequest, ipAddress);
            return Ok();
        }
    }
}