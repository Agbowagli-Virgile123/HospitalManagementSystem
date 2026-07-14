using HospitalManagementSystem.Models.Appointment;
using Microsoft.Data.SqlClient;

namespace HospitalManagementSystem.Repository
{
    public interface IAppointmentRepo
    {
        Task<List<RecentAppointment>> GetRecentAppointmentsAsync(DateTime? fromDate = null, DateTime? toDate = null);
    }

    public class AppointmentRepository(IConfiguration config) : IAppointmentRepo
    {
        private readonly string _connectionString = config.GetConnectionString("DefaultConnection")!;

        public async Task<List<RecentAppointment>> GetRecentAppointmentsAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var appointments = new List<RecentAppointment>();

            const string sql = @"
                    SELECT
                        a.Id,
                        a.Reason,
                        a.Status,
                        a.AppointmentDate,
                        a.AppointmentTime,
                        a.CreatedAt,
                        a.UpdatedAt,

                        p.Id AS PatientId,
                        p.FirstName + ' ' + p.LastName AS PatientName,

                        d.Id AS DoctorId,
                        d.FirstName + ' ' + d.LastName AS DoctorName

                    FROM Appointments a
                    INNER JOIN Patients p
                        ON a.PatientId = p.Id
                    INNER JOIN Doctors d
                        ON a.DoctorId = d.Id

                    WHERE
                        (
                            @FromDate IS NULL
                            AND @ToDate IS NULL
                            AND a.AppointmentDate = CAST(GETDATE() AS DATE)
                        )
                        OR
                        (
                            (@FromDate IS NULL OR a.AppointmentDate >= @FromDate)
                            AND (@ToDate IS NULL OR a.AppointmentDate <= @ToDate)
                            AND NOT (@FromDate IS NULL AND @ToDate IS NULL)
                        )

                    ORDER BY
                        a.AppointmentDate DESC,
                        a.AppointmentTime DESC;
                ";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@FromDate", (object?)fromDate?.Date ?? DBNull.Value);
            command.Parameters.AddWithValue("@ToDate",(object?)toDate?.Date ?? DBNull.Value);

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                appointments.Add(new RecentAppointment
                {
                    Id = reader["Id"].ToString()!,
                    Reason = reader["Reason"]?.ToString() ?? "",
                    Status = reader["Status"].ToString()!,

                    Date = Convert.ToDateTime(reader["AppointmentDate"])
                        .ToString("yyyy-MM-dd"),

                    Time = TimeOnly
                        .FromTimeSpan((TimeSpan)reader["AppointmentTime"])
                        .ToString("hh\\:mm"),

                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),

                    UpdatedAt = reader["UpdatedAt"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["UpdatedAt"]),

                    PatientId = reader["PatientId"].ToString()!,
                    PatientName = reader["PatientName"].ToString()!,

                    DoctorId = reader["DoctorId"].ToString()!,
                    DoctorName = reader["DoctorName"].ToString()!
                });
            }

            return appointments;
        }
    }
}
