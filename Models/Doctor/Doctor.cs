using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HospitalManagementSystem.Models.Doctor
{
    public class Doctor
    {
        public string Id { get; set; } =string.Empty;

        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";

        public string Gender { get; set; } = "";

        public string Specialization { get; set; } = "";

        public string PhoneNumber { get; set; } = "";

        public string Email { get; set; } = "";

        public decimal ConsultationFee { get; set; }

        public bool IsAvailable { get; set; }
        
        public string? DepartementId { get; set; } // to be added
        public string? LicenseNumber { get; set; } // to be added

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
