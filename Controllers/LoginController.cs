using Microsoft.AspNetCore.Mvc;
using ATMBank.Models;
using ATMBank.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System;

namespace ATMBank.Controllers
{
    [ApiController]
    [Route("api/logins")]
    public class LoginController : ControllerBase
    {
        private readonly ATMContext _context;

        public LoginController(ATMContext context)
        {
            _context = context;
        }

        // API đăng nhập
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            // Kiểm tra người dùng có tồn tại hay không
            var user = _context.Users.FirstOrDefault(u => u.Email == loginDto.Email);            if (user == null)
            {
                return Unauthorized("Tên người dùng hoặc mật khẩu không đúng");
            }

            // Kiểm tra mật khẩu có đúng không
            var passwordHash = HashPassword(loginDto.Password);
            if (passwordHash != user.Password)
            {
                return Unauthorized("Tên người dùng hoặc mật khẩu không đúng");
            }

            // Nếu đăng nhập thành công, trả về thông tin người dùng và token giả
            var userResponse = new
            {
               UserId = user.UserId,         // ID người dùng
               Name = user.Name,         // Tên người dùng
               Email = user.Email, 
                //Avatar = user.Avatar,  // Avatar người dùng (nếu có)
                Token = GenerateFakeJwtToken() // Token giả (có thể thay thế bằng JWT thực tế)
            };

            return Ok(userResponse);
        }

        // Hàm mã hóa mật khẩu (sử dụng SHA256)
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        // Tạo token giả (có thể thay thế bằng JWT thực tế)
        private string GenerateFakeJwtToken()
        {
            // Token giả, có thể thay thế bằng JWT thực tế sau khi cấu hình xác thực
            return "fake-jwt-token";
        }
    }
}
