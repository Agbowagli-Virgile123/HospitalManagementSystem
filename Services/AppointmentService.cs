using HospitalManagementSystem.Models.Appointment;
using HospitalManagementSystem.Repository;

namespace HospitalManagementSystem.Services
{
    public interface IAppointmentService
    {
        Task<List<RecentAppointment>> GetRecentAppointmentsAsync(DateTime? fromDate = null, DateTime? toDate = null);
    };

    public class AppointmentService(IAppointmentRepo repo) : IAppointmentService
    {
        public async Task<List<RecentAppointment>> GetRecentAppointmentsAsync(DateTime? fromDate = null, DateTime? toDate = null)
            => await repo.GetRecentAppointmentsAsync(fromDate, toDate);
        
    }
}
