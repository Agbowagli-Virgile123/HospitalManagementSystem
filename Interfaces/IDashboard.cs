using HospitalManagementSystem.Models.Dashboard;

namespace HospitalManagementSystem.Interfaces
{
    public interface IDashboard
    {
        Task<DashboardSummary> GetDashboardSummaryAsync();
    }
}
