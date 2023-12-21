using AuthService.Models.Dtos;
using AuthService.Services;
using AuthService.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMessageBus;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ResponseDto _response;
        private readonly IConfiguration _configuration;

        public UserController(UserService user, IConfiguration configuration) 
        {
            _userService = user;
            _configuration = configuration;
            _response = new ResponseDto();
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ResponseDto>> RegisterUser(RegisterUserDto registerUserDto)
        {
            var res= await _userService.RegisterUser(registerUserDto);


            if(string.IsNullOrWhiteSpace(res))
            {
                // success
                _response.Result = "User Registered Successfully";
                // add message to queue

                var message = new UserMessageDto()
                {
                    Name = registerUserDto.Name,
                    Email = registerUserDto.Email,
                };

                var mb = new MessageBus();
                await mb.PublishMessage(message, _configuration.GetValue<string>("ServiceBus:register"));

                return Created("", _response);
            }

            _response.Errormessage = res;
            _response.IsSuccess = false;
            return BadRequest(_response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ResponseDto>> loginUser(LoginRequestDto loginRequestDto)
        {
            var res = await _userService.loginUser(loginRequestDto);


            if (res.User != null)
            {
                // success
                _response.Result = res;
                return Created("", _response);
            }

            _response.Errormessage = "Invalid Credentials";
            _response.IsSuccess = false;
            return BadRequest(_response);
        }
    }
}
