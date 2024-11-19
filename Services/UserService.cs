// Services/UserService.cs
using ATMBank.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ATMBank.Data;
namespace ATMBank.Services
{
    public class UserService : IUserService
    {
        private readonly ATMContext _context;

        public UserService(ATMContext context)
        {
            _context = context;
        }

        public async Task RegisterAsync(RegisterDto registerDto)
        {
            // Kiểm tra nếu người dùng đã tồn tại
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Name == registerDto.Email);

            if (existingUser != null)
                throw new Exception("User already exists.");

            // Tạo người dùng mới và lưu vào cơ sở dữ liệu
            var newUser = new User
            {
                Name = registerDto.Email,
                Password = registerDto.Password // Mã hóa mật khẩu là tốt nhất, nhưng để demo không mã hóa
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
        }
    }
}
