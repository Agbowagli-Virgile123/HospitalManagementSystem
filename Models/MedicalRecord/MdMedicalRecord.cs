using System.ComponentModel;
using System.Text.RegularExpressions;

namespace HospitalManagementSystem.Models.MedicalRecord
{
    public class MdPostMedicalRecord
    {
        public string? PatientId { get; set; }
        public DateTime RecordDate { get; set; } = DateTime.Now;
        public string? Temperature { get; set; }
        public string? Pulse { get; set; }
        public string? BloodPressure { get; set; }
        public string? Weight { get; set; }
        public string? Height { get; set; }
        public string? BloodGroup { get; set; }
        public string? DoctorId { get; set; }
        public string? AppointmentId { get; set; }
        public string? Symptoms  { get; set; }
        public string? Diagnosis { get; set; }
        public string? Note { get; set; }
        public string? Treatment { get; set; }
        public string? Prescription { get; set; }
        public string? LaboratoryTest { get; set; }
        public DateTime? NextVisit { get; set; }
        public string? Remarks { get; set; }

    }

    public class MdGetMedicaRecord : MdPostMedicalRecord
    {

        public string? Id { get; set; }
        public string? PatientName { get; set; }
        public string? DoctorName { get; set; }
        public string? AppointmentName { get; set; }
        public string? PatientNumber { get; set; }



    }
}
