
namespace HospitalManagementSystem.Models.Prescription
{
    public class MdPostPrescription
    {
        public string? PrescriptionNumber { get; set; }
        public DateTime PrescriptionDate {get;set;} = DateTime.Now;
        public string? Status { get; set; }
        public string? PatientId {get;set;}
        public string? DoctorId {get;set;}
        public string? MedicalRecordId {get;set;}
        public string? MedicationId {get;set;}
        public string? Dosage {get;set;}
        public double NumberOfDays {get;set;}
        public string? Note {get;set;}
        public double TotalItems { get;set;}
      

    }

    public class MdGetPrescription : MdPostPrescription
    {
        public string? Id { get; set; }
        public string? PatientName { get; set; }
        public string? DoctorName { get; set; }
        public string? MedicalRecordName { get; set; }
        public string? MedicationName { get; set; }

    }

    public class MdGetPrescriptionItem
    { 
        public double Quantity { get; set; }
    }
}
