using HospitalManagementSystem.Interfaces;
using HospitalManagementSystem.Models.Dashboard;
using Microsoft.Data.SqlClient;

namespace HospitalManagementSystem.Services
{
    public class DashboardService(IConfiguration config) : IDashboard
    {
        private readonly string _connectionString = config.GetConnectionString("DefaultConnection")!;

        public async Task<DashboardSummary> GetDashboardSummaryAsync()
        {
            const string sql = @"
                    SELECT
                        (SELECT COUNT(*) FROM Doctors) AS TotalDoctors,

                        (SELECT COUNT(*) FROM Patients) AS TotalPatients,

                        (SELECT COUNT(*) FROM Appointments) AS TotalAppointments,

                        (
                            SELECT ISNULL(SUM(TotalAmount), 0)
                            FROM Billing
                            WHERE PaymentStatus = 'Paid'
                        ) AS TotalRevenue,

                        (
                            SELECT COUNT(*)
                            FROM Appointments
                            WHERE AppointmentDate = CAST(GETDATE() AS DATE)
                        ) AS TodayAppointments,

                        (
                            SELECT COUNT(*)
                            FROM Appointments
                            WHERE Status = 'Pending'
                        ) AS PendingAppointments,

                        (
                            SELECT COUNT(*)
                            FROM Appointments
                            WHERE Status = 'Completed'
                        ) AS CompletedAppointments,

                        (
                            SELECT COUNT(*)
                            FROM Appointments
                            WHERE Status = 'Cancelled'
                        ) AS CancelledAppointments;
                ";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new DashboardSummary
                {
                    TotalDoctors = Convert.ToInt32(reader["TotalDoctors"]),
                    TotalPatients = Convert.ToInt32(reader["TotalPatients"]),
                    TotalAppointments = Convert.ToInt32(reader["TotalAppointments"]),
                    TotalRevenue = Convert.ToDecimal(reader["TotalRevenue"]),
                    TodayAppointments = Convert.ToInt32(reader["TodayAppointments"]),
                    PendingAppointments = Convert.ToInt32(reader["PendingAppointments"]),
                    CompletedAppointments = Convert.ToInt32(reader["CompletedAppointments"]),
                    CancelledAppointments = Convert.ToInt32(reader["CancelledAppointments"])
                };
            }

            return new DashboardSummary();
        }
    }
}
