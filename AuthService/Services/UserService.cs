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
        private readonly IJwt _jwtServices;

        public UserService(ApplicationDbContext applicationDbContext, IMapper mapper, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IJwt jwtService)
        {
            _context = applicationDbContext;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtServices = jwtService;
        }
           
            public async Task<bool> AssignUserRoles(string Email, string RoleName)
        {
            var user = await _context.ApplicationUsers.Where(x => x.Email.ToLower() == Email.ToLower()).FirstOrDefaultAsync();
            //does  the user exist?
            if (user == null)
            {
                return false;
            }
            else
            {
                //does the role exist 
                if (!_roleManager.RoleExistsAsync(RoleName).GetAwaiter().GetResult())
                {
                    //create the role 
                    await _roleManager.CreateAsync(new IdentityRole(RoleName));
                }

                //assign the user the role
                await _userManager.AddToRoleAsync(user, RoleName);
                return true;
            }

        }

        public async Task<ApplicationUser> GetUserById(string Id)
        {
            return await _context.ApplicationUsers.Where(x => x.Id == Id).FirstOrDefaultAsync();
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


