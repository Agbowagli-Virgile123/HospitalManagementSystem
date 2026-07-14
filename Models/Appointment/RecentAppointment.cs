namespace HospitalManagementSystem.Models.Appointment
{
    public class RecentAppointment
    {
        // appointment
        public string Id { get; set; } = string.Empty;
        public string Reason {  get; set; } = string.Empty;
        public string Status {  get; set; } = string.Empty;
        public string Time {  get; set; } = string.Empty;
        public string Date {  get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // patient
        public string PatientId { get; set; } = string.Empty;
        public string PatientName { get; set;} = string.Empty;

        // doctor
        public string DoctorId {  get; set; } = string.Empty;
        public string DoctorName {  get; set; } = string.Empty;
    }
}
