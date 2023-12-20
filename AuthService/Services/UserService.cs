using AuthService.Models.Dtos;
using AuthService.Data;
using AuthService.Services.IServices;
using AuthService.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

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

        public Task<LoginRequestDto> loginUser(LoginRequestDto loginRequestDto)
        {
            throw new NotImplementedException();
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
    }
}
