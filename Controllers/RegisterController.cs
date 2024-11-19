using ATMBank.Models;
using ATMBank.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ATMBank.Controllers
{
    [ApiController]
    [Route("api/registers")]
    public class RegisterController : ControllerBase
    {
        private readonly ATMContext _context;

        public RegisterController(ATMContext context)
        {
            _context = context;
        }

        // API đăng ký
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto registerDto)
        {
            // Kiểm tra nếu người dùng đã tồn tại theo tên người dùng hoặc email
            if (_context.Users.Any(u => u.Name == registerDto.Name))
            {
                return BadRequest("Tên người dùng đã tồn tại");
            }
            
            if (_context.Users.Any(u => u.Email == registerDto.Email))
            {
                return BadRequest("Email đã tồn tại");
            }

            // Mã hóa mật khẩu
            var passwordHash = HashPassword(registerDto.Password);

            // Tạo người dùng mới
            var newUser = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,  // Save Email
                Password = passwordHash
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok("Đăng ký thành công");
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
