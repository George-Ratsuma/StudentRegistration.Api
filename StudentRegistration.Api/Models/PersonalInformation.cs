namespace StudentRegistration.Api.Models
{
    public class PersonalInformation
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public string IdNumber { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        public string Gender { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public bool HasDisability { get; set; }

        public string? DisabilityDescription { get; set; }

        public string NextOfKinName { get; set; } = string.Empty;

        public string NextOfKinRelationship { get; set; } = string.Empty;

        public string NextOfKinPhone { get; set; } = string.Empty;
    }
}