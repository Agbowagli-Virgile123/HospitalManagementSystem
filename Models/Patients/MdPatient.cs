using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace HospitalManagementSystem.Models.Patients
{
    public class MdGetPatient : MdPostPatient
    {
        public DateTime CreatedAt { get; set; }
    }


    public class MdPostPatient
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public string? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Status { get; set; }
        public string? EmergencyName { get; set; }
        public string? EmergencyContactPhone { get; set; }
        public string? BloodGroup { get; set; }
        public string? MaritalStatus { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Nationality { get; set; }
        public string? AllergyDescription { get; set; }
        public DateOnly DOB { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }

}
