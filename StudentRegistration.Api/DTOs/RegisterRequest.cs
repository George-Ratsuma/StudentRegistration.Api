using System.ComponentModel.DataAnnotations;

namespace StudentRegistration.Api.DTOs
{
    public class RegisterRequest
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string FirstNames { get; set; } = string.Empty;

        [Required]
        public string Surname { get; set; } = string.Empty;

        [Required]
        public string Street { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string State { get; set; } = string.Empty;

        [Required]
        public string ZipCode { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? HighestQualification { get; set; }

        [Required]
        [MinLength(8)]
        [RegularExpression(
    @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).+$",
    ErrorMessage = "Password must contain at least 8 characters, including an uppercase letter, lowercase letter, number, and special character.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}