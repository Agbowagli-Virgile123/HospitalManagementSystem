using HospitalManagementSystem.Models.Doctor;
using Microsoft.Data.SqlClient;

namespace HospitalManagementSystem.Repository
{
    public interface IDoctorRepo
    {
        Task<bool> AddDoctorAsync(DoctorPost doctor);
        Task<bool> UpdateDoctorAsync(DoctorGet doctor);
        Task<List<DoctorGet>> GetDoctorsAsync(string? search = null);
        Task<DoctorGet?> GetDoctorAsync(string doctorId);
        Task<bool> DeleteDoctorAsync(string doctorId);
    }
    public class DoctorRepository(IConfiguration  config) : IDoctorRepo
    {
        private readonly string _connectionString = config.GetConnectionString("DefaultConnection")!;

        // GET
        public async Task<DoctorGet?> GetDoctorAsync(string doctorId)
        {
            const string sql = @"
                SELECT 
                    doc.*, 
                    dep.DeptName
                FROM Doctors AS doc
                JOIN Department AS dep
                    ON doc.DepartmentId = dep.Id
                WHERE doc.Id = @Id;
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
        public async Task<List<DoctorGet>> GetDoctorsAsync(string? search = null)
        {
            var doctors = new List<DoctorGet>();

            const string sql = @"
                    SELECT 
                        doc.*,
                        dep.DeptName
                    FROM Doctors AS doc
                    JOIN Department AS dep
                        ON doc.DepartmentId = dep.Id
                    WHERE
                        @Search IS NULL
                        OR @Search = ''
                        OR doc.FirstName LIKE '%' + @Search + '%'
                        OR doc.LastName LIKE '%' + @Search + '%'
                        OR (doc.FirstName + ' ' + doc.LastName) LIKE '%' + @Search + '%'
                    ORDER BY doc.FirstName, doc.LastName;
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
        public async Task<bool> AddDoctorAsync(DoctorPost doctor)
        {
            const string sql = @"
                INSERT INTO Doctors
                (
                    Id,
                    FirstName,
                    LastName,
                    Gender,
                    Specialization,
                    LicenseNumber,
                    DepartmentId,
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
                    @LicenseNumber,
                    @DepartmentId,
                    @PhoneNumber,
                    @Email,
                    @ConsultationFee,
                    @IsAvailable
                );
            ";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Id", Guid.NewGuid().ToString());
            command.Parameters.AddWithValue("@FirstName", doctor.FirstName);
            command.Parameters.AddWithValue("@LastName", doctor.LastName);
            command.Parameters.AddWithValue("@DepartmentId", doctor.DepartementId);
            command.Parameters.AddWithValue("@LicenseNumber", doctor.LicenseNumber);
            command.Parameters.AddWithValue("@Gender", (object?)doctor.Gender ?? DBNull.Value);
            command.Parameters.AddWithValue("@Specialization", doctor.Specialization);
            command.Parameters.AddWithValue("@PhoneNumber", (object?)doctor.PhoneNumber ?? DBNull.Value);
            command.Parameters.AddWithValue("@Email", (object?)doctor.Email ?? DBNull.Value);
            command.Parameters.AddWithValue("@ConsultationFee", doctor.ConsultationFee);
            command.Parameters.AddWithValue("@IsAvailable", doctor.IsAvailable);

            return await command.ExecuteNonQueryAsync() > 0;
        }
        public async Task<bool> UpdateDoctorAsync(DoctorGet doctor)
        {
            const string sql = @"
                    UPDATE Doctors
                    SET
                        FirstName = @FirstName,
                        LastName = @LastName,
                        Gender = @Gender,
                        DepartmentId = @DepartmentId,
                        LicenseNumber = @LicenseNumber,
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
            command.Parameters.AddWithValue("@DepartmentId", doctor.DepartementId);
            command.Parameters.AddWithValue("@LicenseNumber", doctor.LicenseNumber);
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

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            //check if the dorctor id is in the departementid
            string ckeckquery = @"SELECT COUNT(*) FROM Department WHERE HeadDoctorId = @HeadDoctorId";

            using var checkcommand = new SqlCommand(ckeckquery, connection);
            checkcommand.Parameters.AddWithValue("@HeadDoctorId", doctorId);
            int count = Convert.ToInt16(await checkcommand.ExecuteScalarAsync());


            if (count > 0)
            {
                return false;
            }
                

            const string sql = @"
                    DELETE FROM Doctors
                    WHERE Id = @Id;
                ";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", doctorId);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        // Helpers
        private static DoctorGet MapDoctor(SqlDataReader reader)
        {
            return new DoctorGet
            {
                Id = reader["Id"].ToString()!,
                FirstName = reader["FirstName"].ToString()!,
                LastName = reader["LastName"].ToString()!,
                LicenseNumber = reader["LicenseNumber"].ToString()!,
                DepartementId = reader["DepartmentId"].ToString()!,
                DepartmentName = reader["DeptName"].ToString()!,
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
