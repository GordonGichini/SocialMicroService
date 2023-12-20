using AuthService.Models.Dtos;
using AuthService.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _userService;
        private readonly ResponseDto _response;

        public UserController(IUser user) 
        {
            _userService = user;
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
                return Created("", _response);
            }

            _response.Errormessage = res;
            _response.IsSuccess = false;
            return BadRequest(_response);
        }
    }
}
