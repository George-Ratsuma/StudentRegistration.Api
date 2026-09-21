using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using StudentRegistration.Api.Data;
using StudentRegistration.Api.DTOs;

namespace StudentRegistration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (studentId == null)
            {
                return Unauthorized();
            }

            var student = await _context.Students
                .FindAsync(int.Parse(studentId));

            if (student == null)
            {
                return NotFound();
            }

             

            return Ok(new
            {
                student.id,
                student.Title,
                student.FirstNames,
                student.Surname,
                student.Street,
                student.City,
                student.State,
                student.ZipCode,
                student.Email,
                student.HighestQualification
            });
        }
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile(
    UpdateStudentRequest request)
        {
            var studentId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (studentId == null)
            {
                return Unauthorized();
            }

            var student = await _context.Students
                .FindAsync(int.Parse(studentId));

            if (student == null)
            {
                return NotFound();
            }

            var emailInUse = await _context.Students
                .AnyAsync(s =>
                    s.Email == request.Email &&
                    s.id != student.id);

            if (emailInUse)
            {
                return BadRequest("Email is already registered.");
            }

            student.Title = request.Title;
            student.FirstNames = request.FirstNames;
            student.Surname = request.Surname;
            student.Street = request.Street;
            student.City = request.City;
            student.State = request.State;
            student.ZipCode = request.ZipCode;
            student.Email = request.Email;
            student.HighestQualification =
                request.HighestQualification;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Profile updated successfully."
            });
        }
    }
}