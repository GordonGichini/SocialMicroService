using AuthService.Models;

namespace AuthService.Services.IServices
{
    public interface IJwt
    {
        string GenerateToken(ApplicationUser user, IEnumerable<string> Roles);
    }
}
