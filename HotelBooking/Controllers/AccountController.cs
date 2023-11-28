using AutoMapper;
using HotelBooking.AuthServices;
using HotelBooking.DTO;
using HotelBooking.EmailService;
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
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<ApiUser> userManager, IMapper mapper, ILogger<AccountController> logger, IAuth auth, IEmailSender emailSender)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _auth = auth;
            _emailSender = emailSender;
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

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(VerifyEmail), "Account", new { token, email = user.Email });

            var message = new Message(new string[] {user.Email}, "Please check your email for Confirmation link", confirmationLink, null);
            await _emailSender.SendEmailAsync(message);

            await _userManager.AddToRolesAsync(user, userDTO.Roles);

            return Accepted();

        }

        [HttpGet]
        [Route("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailDTO verifyEmailDTO)
        {
            var user = await _userManager.FindByEmailAsync(verifyEmailDTO.email);

            if (user == null)
            {
                return BadRequest(ModelState);
            }

            var result = await _userManager.ConfirmEmailAsync(user, verifyEmailDTO.token);
            return Ok(result.Succeeded ? nameof(VerifyEmail) : "Error");
        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
 
            _logger.LogInformation($"Login attempt for {loginDTO.Email}!");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _auth.ValidateUser(loginDTO))
            {
                return Unauthorized();
            }

            return Accepted( new { Token = await _auth.CreateToken()});

        }
    }
}
