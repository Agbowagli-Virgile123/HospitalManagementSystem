using HospitalManagementSystem.Models.Doctor;
using Microsoft.Data.SqlClient;

namespace HospitalManagementSystem.Repository
{
    public interface IDoctorRepo
    {
        Task<bool> AddDoctorAsync(Doctor doctor);
        Task<bool> UpdateDoctorAsync(Doctor doctor);
        Task<List<Doctor>> GetDoctorsAsync(string? search = null);
        Task<Doctor?> GetDoctorAsync(string doctorId);
        Task<bool> DeleteDoctorAsync(string doctorId);
    }
    public class DoctorRepository(IConfiguration  config) : IDoctorRepo
    {
        private readonly string _connectionString = config.GetConnectionString("DefaultConnection")!;

        // GET
        public async Task<Doctor?> GetDoctorAsync(string doctorId)
        {
            const string sql = @"
                SELECT *
                FROM Doctors
                WHERE Id = @Id;
            ";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", doctorId);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapDoctor(reader);
            }

            return null;
        }
        public async Task<List<Doctor>> GetDoctorsAsync(string? search = null)
        {
            var doctors = new List<Doctor>();

            const string sql = @"
                    SELECT *
                    FROM Doctors
                    WHERE
                        @Search IS NULL
                        OR FirstName LIKE '%' + @Search + '%'
                        OR LastName LIKE '%' + @Search + '%'
                        OR (FirstName + ' ' + LastName) LIKE '%' + @Search + '%'
                    ORDER BY FirstName, LastName;
                ";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Search", string.IsNullOrWhiteSpace(search) ? DBNull.Value : search.Trim());

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                doctors.Add(MapDoctor(reader));
            }

            return doctors;
        }

        // POST
        public async Task<bool> UpdateDoctorAsync(Doctor doctor)
        {
            const string sql = @"
                    UPDATE Doctors
                    SET
                        FirstName = @FirstName,
                        LastName = @LastName,
                        Gender = @Gender,
                        Specialization = @Specialization,
                        PhoneNumber = @PhoneNumber,
                        Email = @Email,
                        ConsultationFee = @ConsultationFee,
                        IsAvailable = @IsAvailable,
                        UpdatedAt = GETDATE()
                    WHERE Id = @Id;
                ";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Id", doctor.Id);
            command.Parameters.AddWithValue("@FirstName", doctor.FirstName);
            command.Parameters.AddWithValue("@LastName", doctor.LastName);
            command.Parameters.AddWithValue("@Gender", (object?)doctor.Gender ?? DBNull.Value);
            command.Parameters.AddWithValue("@Specialization", doctor.Specialization);
            command.Parameters.AddWithValue("@PhoneNumber", (object?)doctor.PhoneNumber ?? DBNull.Value);
            command.Parameters.AddWithValue("@Email", (object?)doctor.Email ?? DBNull.Value);
            command.Parameters.AddWithValue("@ConsultationFee", doctor.ConsultationFee);
            command.Parameters.AddWithValue("@IsAvailable", doctor.IsAvailable);

            return await command.ExecuteNonQueryAsync() > 0;
        }
        public async Task<bool> AddDoctorAsync(Doctor doctor)
        {
            const string sql = @"
                INSERT INTO Doctors
                (
                    Id,
                    FirstName,
                    LastName,
                    Gender,
                    Specialization,
                    PhoneNumber,
                    Email,
                    ConsultationFee,
                    IsAvailable
                )
                VALUES
                (
                    @Id,
                    @FirstName,
                    @LastName,
                    @Gender,
                    @Specialization,
                    @PhoneNumber,
                    @Email,
                    @ConsultationFee,
                    @IsAvailable
                );
            ";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Id", doctor.Id);
            command.Parameters.AddWithValue("@FirstName", doctor.FirstName);
            command.Parameters.AddWithValue("@LastName", doctor.LastName);
            command.Parameters.AddWithValue("@Gender", (object?)doctor.Gender ?? DBNull.Value);
            command.Parameters.AddWithValue("@Specialization", doctor.Specialization);
            command.Parameters.AddWithValue("@PhoneNumber", (object?)doctor.PhoneNumber ?? DBNull.Value);
            command.Parameters.AddWithValue("@Email", (object?)doctor.Email ?? DBNull.Value);
            command.Parameters.AddWithValue("@ConsultationFee", doctor.ConsultationFee);
            command.Parameters.AddWithValue("@IsAvailable", doctor.IsAvailable);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        // Deleting
        public async Task<bool> DeleteDoctorAsync(string doctorId)
        {
            const string sql = @"
                    DELETE FROM Doctors
                    WHERE Id = @Id;
                ";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", doctorId);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        // Helpers
        private static Doctor MapDoctor(SqlDataReader reader)
        {
            return new Doctor
            {
                Id = reader["Id"].ToString()!,
                FirstName = reader["FirstName"].ToString()!,
                LastName = reader["LastName"].ToString()!,
                Gender = reader["Gender"] == DBNull.Value ? null : reader["Gender"].ToString()!,
                Specialization = reader["Specialization"].ToString()!,
                PhoneNumber = reader["PhoneNumber"] == DBNull.Value ? null : reader["PhoneNumber"].ToString()!,
                Email = reader["Email"] == DBNull.Value? null : reader["Email"].ToString()!,
                ConsultationFee = Convert.ToDecimal(reader["ConsultationFee"]),
                IsAvailable = Convert.ToBoolean(reader["IsAvailable"]),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? null : Convert.ToDateTime(reader["UpdatedAt"])
            };
        }
    }
}
