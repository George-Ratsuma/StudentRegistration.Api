using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRegistration.Api.Data;
using StudentRegistration.Api.DTOs;
using StudentRegistration.Api.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using StudentRegistration.Api.Services;
using System.Text;


namespace StudentRegistration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        public AuthController(AppDbContext context, IConfiguration configuration, EmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var existingStudent = await _context.Students
                .FirstOrDefaultAsync(s => s.Email == request.Email);

            if (existingStudent != null)
            {
                return BadRequest("Email is already registered.");
            }

            if (request.Password.Length < 8)
            {
                return BadRequest("Password must be at least 8 characters long.");
            }

            if (request.Password.Equals(request.Email, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Password cannot be the same as your email.");
            }

            if (request.Password != request.ConfirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }

            var student = new Student
            {
                Title = request.Title,
                FirstNames = request.FirstNames,
                Surname = request.Surname,
                Street = request.Street,
                City = request.City,
                State = request.State,
                ZipCode = request.ZipCode,
                Email = request.Email,
                HighestQualification = request.HighestQualification,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                IsEmailVerified = false,
                EmailVerificationToken = Guid.NewGuid().ToString()
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            var verificationLink =
    $"http://localhost:4200/verify-email?token={student.EmailVerificationToken}";

            await _emailService.SendVerificationEmail(
                student.Email,
                student.FirstNames,
                verificationLink);

            return Ok(new
            {
                message = "Registration successful."
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Email == request.Email);

            if (student == null)
            {
                return Unauthorized("Invalid email or password.");
            }
            if (!student.IsEmailVerified)
            {
                return Unauthorized(new
                {
                    message = "Please verify your email before logging in."
                });
            }
            var passwordValid = BCrypt.Net.BCrypt.Verify(
                request.Password,
                student.PasswordHash);

            if (!passwordValid)
            {
                return Unauthorized("Invalid email or password.");
            }

            var claims = new[]
{
    new Claim(ClaimTypes.NameIdentifier, student.id.ToString()),
    new Claim(ClaimTypes.Email, student.Email)
};

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.EmailVerificationToken == token);

            if (student == null)
            {
                return BadRequest("Invalid verification token.");
            }

            if (student.IsEmailVerified)
            {
                return BadRequest("Email is already verified.");
            }

            student.IsEmailVerified = true;
            student.EmailVerificationToken = null;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Email verified successfully."
            });
        }
    }
}