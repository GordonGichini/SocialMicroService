using AuthService.Models.Dtos;
using AuthService.Data;
using AuthService.Services.IServices;
using AuthService.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services
{
    public class UserService : IUser
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public UserService(ApplicationDbContext applicationDbContext, IMapper mapper, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = applicationDbContext;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }
           
            public Task<bool> AssignUserRoles(string Email, string RoleName)
        {
            throw new NotImplementedException();
        }

        public async Task<LoginResponseDto> loginUser(LoginRequestDto loginRequestDto)
        {
            // a User witht that username exists
            var user = await _context.ApplicationUsers.Where(x => x.UserName.ToLower() == loginRequestDto.UserName.ToLower()).FirstOrDefaultAsync();
            // compare hashed password with plain text password
            var isValid = _userManager.CheckPasswordAsync(user, loginRequestDto.Password).GetAwaiter().GetResult();

            if (!isValid || user == null)
            {
                // if username or password are wrong
                return new LoginResponseDto();
            }
            var loggeduser = _mapper.Map<UserDto>(user);

            var response = new LoginResponseDto()
            {
                User = loggeduser,
                Token = "Coming soon..."
            };
            return response;
        }

        public async Task<string> RegisterUser(RegisterUserDto userDto)
        {
            try
            {
                var user = _mapper.Map<ApplicationUser>(userDto);
               

                // create user
                var result = await _userManager.CreateAsync(user, userDto.Password);

                // if this succeeded
                if(result.Succeeded)
                {
                    return string.Empty;
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        Task<LoginRequestDto> IUser.loginUser(LoginRequestDto loginRequestDto)
        {
            throw new NotImplementedException();
        }
    }
}


