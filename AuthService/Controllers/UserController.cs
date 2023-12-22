using AuthService.Models.Dtos;
using AuthService.Services;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public UserController(UserService user, IConfiguration configuration, IMapper mapper) 
        {
            _userService = user;
            _configuration = configuration;
            _response = new ResponseDto();
            _mapper = mapper;
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

        [HttpPost("AssignRole")]
        public async Task<ActionResult<ResponseDto>> AssignRole(AssignRoleDto role)
        {
            var res = await _userService.AssignUserRoles(role.Email, role.Role);

            if (res)
            {
                //this was success
                _response.Result = res;
                return Ok(_response);
            }

            _response.Errormessage = "Error Occured ";
            _response.Result = res;
            _response.IsSuccess = false;
            return BadRequest(_response);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<ResponseDto>> GetUser(string Id)
        {
            var res = await _userService.GetUserById(Id);
            var user = _mapper.Map<UserDto>(res);
            if (res != null)
            {
                //this was success
                _response.Result = user;
                return Ok(_response);
            }

            _response.Errormessage = "User Not found ";
            _response.IsSuccess = false;
            return NotFound(_response);
        }
    }
}
