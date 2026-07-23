namespace HospitalManagementSystem.Models.Appointment
{
    public class MdPostSiteAppointment : MdPostAppointment
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
    public class MdPostAppointment
    {
        public string PatientId { get; set; } = string.Empty;
        public string DepartmentId { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public string? DoctorId { get; set; }
        public string? Reason { get; set; }
        public string? Note { get; set; }
        public DateOnly AppointmentDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public TimeOnly AppointmentTime { get; set; } = TimeOnly.FromDateTime(DateTime.Now);
    }

    public class MdGetAppointment : MdPostAppointment
    {
        public string? Id {get; set;}
        public string? PatientName {get; set;}
        public string? PatientNumber {get; set;}
        public string? DoctorName {get; set;}
        public string? DepartmentName {get; set;}

    }
}
