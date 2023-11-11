using AutoMapper;
using HotelBooking.AuthServices;
using HotelBooking.DTO;
using HotelBooking.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace HotelBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IAuth _auth;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;

        public AccountController(UserManager<ApiUser> userManager, IMapper mapper, ILogger<AccountController> logger, IAuth auth)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _auth = auth;

        }

        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            _logger.LogInformation($"Registration attempt for {userDTO.Email}!");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = _mapper.Map<ApiUser>(userDTO);
                user.UserName = userDTO.Email;
                var result = await _userManager.CreateAsync(user, userDTO.Password);

                if (!result.Succeeded)
                {
                    //Give exact Error message info on the action performed 
                    foreach (var error in result.Errors){
                       ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }

                await _userManager.AddToRolesAsync(user, userDTO.Roles);
                return Accepted();

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong In The {nameof(Register)}");
                return Problem($"$\"Something Went Wrong In The {nameof(Register)}", statusCode: 500);
            }
        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            _logger.LogInformation($"Registration attempt for {loginDTO.Email}!");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                if (!await _auth.ValidateUser(loginDTO))
                {
                    return Unauthorized();
                }

                return Accepted( new {token = await _auth.CreateToken()});

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong In The {nameof(Login)}");
                return Problem($"Something Went Wrong In The {nameof(Login)}", statusCode: 500);
            }
        }
    }
}
