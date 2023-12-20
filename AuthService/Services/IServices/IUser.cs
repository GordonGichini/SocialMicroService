using AuthService.Models.Dtos;


namespace AuthService.Services.IServices
{
    public interface IUser
    {
        Task<string> RegisterUser(RegisterUserDto userDto);
        
        Task<LoginRequestDto> loginUser(LoginRequestDto loginRequestDto);

        Task<bool> AssignUserRoles(string Email, string RoleName);
    }
}
