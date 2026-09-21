namespace StudentRegistration.Api.DTOs
{
    public class EducationHistoryRequest
    {
        public string Institution { get; set; } = string.Empty;

        public string Qualification { get; set; } = string.Empty;

        public string FieldOfStudy { get; set; } = string.Empty;

        public int StartYear { get; set; }

        public int EndYear { get; set; }
    }
}