// Services/IUserService.cs
using ATMBank.Models; // Thêm dòng này
// Services/IUserService.cs
namespace ATMBank.Services
{
    public interface IUserService
    {
        Task RegisterAsync(RegisterDto registerDto);
    }
}
