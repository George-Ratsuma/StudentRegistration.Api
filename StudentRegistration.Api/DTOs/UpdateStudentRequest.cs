namespace StudentRegistration.Api.DTOs
{
    public class UpdateStudentRequest
    {
        public string Title { get; set; } = string.Empty;

        public string FirstNames { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        public string Street { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string ZipCode { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? HighestQualification { get; set; }
    }
}