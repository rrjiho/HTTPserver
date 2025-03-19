using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServerAPI.DB;
using ServerAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ServerAPI.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        // 이것도 DI Container가 bulider 해놓은 context 객체와 참조 연결해줌 IConfiguration 마찬가지 원리
        public UserService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task RegisterAsync(UserRegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                throw new Exception("Username already exists.");

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            var resources = new Resources { UserId = user.Id };
            user.Resources = resources;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<string> LoginAsync(UserLoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid username or password");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<User> GetProfileAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException("User ID cannot be null or empty");

            var user = await _context.Users.FindAsync(int.Parse(userId));
            if (user == null) throw new Exception("User not found");
            return user;
        }

        public async Task AddExperienceAsync(string userId, int exp)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException("User ID cannot be null or empty");

            var user = await _context.Users.FindAsync(int.Parse(userId));
            if (user == null) throw new Exception("User not found");

            user.Experience += exp;
            if(user.Experience >= 100)
            {
                user.Level++;
                user.Experience -= 100;
                user.Strength += 2;
                user.Agility += 2;
                user.Intelligence += 2;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Resources> GetResourcesAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException("User ID cannot be null or empty");

            var resources = await _context.Resources.FirstOrDefaultAsync(r => r.UserId == int.Parse(userId));
            if (resources == null) throw new Exception("Don't have any resources");
            return resources;
        }

        public async Task AddResourcesAsync(string userId, int gold, int gems)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException("User ID cannot be null or empty");

            var resources = await _context.Resources.FirstOrDefaultAsync(r => r.UserId == int.Parse(userId));
            if (resources == null) throw new Exception("Don't have any resources");

            resources.Gold += gold;
            resources.Gems += gems;
            await _context.SaveChangesAsync();
        }

        public async Task UseResourcesAsync(string userId, int gold, int gems)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentException("User ID cannot be null or empty");

            var resources = await _context.Resources.FirstOrDefaultAsync(r => r.UserId == int.Parse(userId));
            if (resources == null) throw new Exception("Don't have any resources");
            if (resources.Gold < gold || resources.Gems < gems) throw new Exception("Insufficient resources");

            resources.Gold -= gold;
            resources.Gems -= gems;
            await _context.SaveChangesAsync();
        }
    }
}
