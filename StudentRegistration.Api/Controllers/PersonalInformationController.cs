using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRegistration.Api.Data;
using StudentRegistration.Api.DTOs;
using StudentRegistration.Api.Models;
using System.Security.Claims;

namespace StudentRegistration.Api.Controllers
{
    [ApiController]
    [Route("api/personal-information")]
    [Authorize]
    public class PersonalInformationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PersonalInformationController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/personal-information
        [HttpGet]
        public async Task<IActionResult> GetPersonalInformation()
        {
            var studentId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var personalInformation = await _context.PersonalInformations
                .FirstOrDefaultAsync(p => p.StudentId == studentId);

            if (personalInformation == null)
            {
                return NotFound(new
                {
                    message = "Personal information has not been added yet."
                });
            }

            return Ok(personalInformation);
        }

        // POST: api/personal-information
        [HttpPost]
        public async Task<IActionResult> AddPersonalInformation(
            PersonalInformationRequest request)
        {
            var studentId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var existingInformation = await _context.PersonalInformations
                .FirstOrDefaultAsync(p => p.StudentId == studentId);

            if (existingInformation != null)
            {
                return Conflict(new
                {
                    message = "Personal information already exists."
                });
            }

            var personalInformation = new PersonalInformation
            {
                StudentId = studentId,
                IdNumber = request.IdNumber,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber,
                HasDisability = request.HasDisability,
                DisabilityDescription = request.DisabilityDescription,
                NextOfKinName = request.NextOfKinName,
                NextOfKinRelationship = request.NextOfKinRelationship,
                NextOfKinPhone = request.NextOfKinPhone
            };

            _context.PersonalInformations.Add(personalInformation);

            await _context.SaveChangesAsync();

            return Ok(personalInformation);
        }

        // PUT: api/personal-information
        [HttpPut]
        public async Task<IActionResult> UpdatePersonalInformation(
            PersonalInformationRequest request)
        {
            var studentId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var personalInformation = await _context.PersonalInformations
                .FirstOrDefaultAsync(p => p.StudentId == studentId);

            if (personalInformation == null)
            {
                return NotFound(new
                {
                    message = "Personal information has not been added yet."
                });
            }

            personalInformation.IdNumber = request.IdNumber;
            personalInformation.DateOfBirth = request.DateOfBirth;
            personalInformation.Gender = request.Gender;
            personalInformation.PhoneNumber = request.PhoneNumber;
            personalInformation.HasDisability = request.HasDisability;
            personalInformation.DisabilityDescription = request.DisabilityDescription;
            personalInformation.NextOfKinName = request.NextOfKinName;
            personalInformation.NextOfKinRelationship = request.NextOfKinRelationship;
            personalInformation.NextOfKinPhone = request.NextOfKinPhone;

            await _context.SaveChangesAsync();

            return Ok(personalInformation);
        }
    }
}