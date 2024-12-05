using ATMBank.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình kết nối tới CSDL
builder.Services.AddDbContext<ATMContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 30)))
);

// Thêm dịch vụ CORS và cấu hình chính sách CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000")  // Địa chỉ ứng dụng React của bạn (hoặc địa chỉ front-end khác)
              .AllowAnyMethod()                    // Cho phép tất cả các phương thức HTTP
              .AllowAnyHeader()                    // Cho phép tất cả các headers
              .AllowCredentials();                 // Cho phép gửi thông tin xác thực (cookies, headers, etc.)
    });
});

// Thêm JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // Chỉ cần false khi đang trong môi trường phát triển, production nên bật true
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])) // Secret key dùng để ký JWT
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Nếu ứng dụng đang chạy trong môi trường phát triển, bật Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Áp dụng chính sách CORS cho toàn bộ ứng dụng
app.UseCors("AllowReactApp");

// Sử dụng Authentication và Authorization
app.UseAuthentication(); // Đảm bảo đặt trước UseAuthorization
app.UseAuthorization();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
