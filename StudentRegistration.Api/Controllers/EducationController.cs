using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using StudentRegistration.Api.Data;
using StudentRegistration.Api.DTOs;
using StudentRegistration.Api.Models;

namespace StudentRegistration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EducationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EducationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEducationHistory()
        {
            var studentId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (studentId == null)
            {
                return Unauthorized();
            }

            var educationHistory = await _context.EducationHistories
                .Where(e => e.StudentId == int.Parse(studentId))
                .ToListAsync();

            return Ok(educationHistory);
        }

        [HttpPost]
        public async Task<IActionResult> AddEducation(
            EducationHistoryRequest request)
        {
            var studentId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (studentId == null)
            {
                return Unauthorized();
            }

            var education = new EducationHistory
            {
                StudentId = int.Parse(studentId),
                Institution = request.Institution,
                Qualification = request.Qualification,
                FieldOfStudy = request.FieldOfStudy,
                StartYear = request.StartYear,
                EndYear = request.EndYear
            };

            _context.EducationHistories.Add(education);

            await _context.SaveChangesAsync();

            return Ok(education);
        }
    }
}