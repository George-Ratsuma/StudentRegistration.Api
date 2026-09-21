namespace StudentRegistration.Api.Models
{
    public class EducationHistory
    {
        public int id { get; set; }

        public int StudentId { get; set; }

        public string Institution { get; set; } = string.Empty;

        public string Qualification { get; set; } = string.Empty;

        public string FieldOfStudy { get; set; } = string.Empty;

        public int StartYear { get; set; }

        public int EndYear { get; set; }
    }
}