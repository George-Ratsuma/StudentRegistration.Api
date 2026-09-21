using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRegistration.Api.Data;
using StudentRegistration.Api.Models;
using System.Security.Claims;

namespace StudentRegistration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CoursesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/courses
        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            var courses = await _context.Courses
                .ToListAsync();

            return Ok(courses);
        }

        // POST: api/courses/{courseId}/register
        [HttpGet("my-courses")]
        public async Task<IActionResult> GetMyCourses()
        {
            var studentId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (studentId == null)
            {
                return Unauthorized();
            }

            var studentIdValue = int.Parse(studentId);

            var myCourses = await _context.StudentCourses
                .Where(sc => sc.StudentId == studentIdValue)
                .Include(sc => sc.Course)
                .Select(sc => sc.Course)
                .ToListAsync();

            return Ok(myCourses);
        }

        [HttpPost("{courseId}/register")]
        public async Task<IActionResult> RegisterForCourse(int courseId)
        {
            var studentId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (studentId == null)
            {
                return Unauthorized();
            }

            var studentIdValue = int.Parse(studentId);

            var course = await _context.Courses
                .FindAsync(courseId);

            if (course == null)
            {
                return NotFound("Course not found.");
            }

            var alreadyRegistered = await _context.StudentCourses
                .AnyAsync(sc =>
                    sc.StudentId == studentIdValue &&
                    sc.CourseId == courseId);

            if (alreadyRegistered)
            {
                return BadRequest(
                    "You are already registered for this course.");
            }

            var studentCourse = new StudentCourse
            {
                StudentId = studentIdValue,
                CourseId = courseId
            };

            _context.StudentCourses.Add(studentCourse);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Successfully registered for the course."
            });
        }
    }
}