using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Model;
using WebApplication1.Services;

namespace WebApplication1.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuthServices _authService;

        public AuthController(AppDbContext context, AuthServices authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register(UserDto request)
        {
            _authService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = request.Role
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("User Registered Successfully");
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginDto request)
        {
            var user = _context.Users.SingleOrDefault(x => x.Username == request.Username);
            if (user == null)
                return BadRequest("User not found");

            if (!_authService.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
                return BadRequest("Wrong password");

            var token = _authService.CreateToken(user);
            return Ok(new { token });
        }



    }

}
