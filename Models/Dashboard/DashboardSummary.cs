namespace HospitalManagementSystem.Models.Dashboard
{
    public class DashboardSummary
    {
        public int TotalDoctors { get; set; }

        public int TotalPatients { get; set; }

        public int TotalAppointments { get; set; }

        public decimal TotalRevenue { get; set; }

        public int TodayAppointments { get; set; }

        public int PendingAppointments { get; set; }

        public int CompletedAppointments { get; set; }

        public int CancelledAppointments { get; set; }
    }
}
