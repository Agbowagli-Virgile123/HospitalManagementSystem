using HospitalManagementSystem.Models.Patients;
using Microsoft.Data.SqlClient;

namespace HospitalManagementSystem.Repository
{
    public interface IPatientRepository
    {
        Task<bool> CreatePatientAsync(MdPostPatient patient);
        Task<bool> UpdatePatientAsync(string patientId, MdPostPatient patient);
        Task<List<MdGetPatient>> GetPatientsAsync(string? search = null);
        Task<MdGetPatient?> GetPatientAsync(string patientId);
    }
    public class PatientRepository(IConfiguration config) : IPatientRepository
    {
        public readonly string _connectionString = config.GetConnectionString("DefaultConnection")!;

        public async Task<bool> CreatePatientAsync(MdPostPatient patient)
        {
            const string sql = @"
            INSERT INTO Patients
            (
                Id,
                FirstName,
                LastName,
                Gender,
                DOB,
                PhoneNumber,
                Email,
                Address,
                EmergencyContactName,
                EmergencyContactPhone,
                BloodGroup,
                MaritalStatus,
                City,
                Country,
                Nationality,
                Status
            )
            VALUES
            (
                @Id,
                @FirstName,
                @LastName,
                @Gender,
                @DOB,
                @PhoneNumber,
                @Email,
                @Address,
                @EmergencyContactName,
                @EmergencyContactPhone,
                @BloodGroup,
                @MaritalStatus,
                @City,
                @Country,
                @Nationality,
                @Status
            );";

            using var connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();

            using var cmd = new SqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@Id", Guid.NewGuid().ToString());
            cmd.Parameters.AddWithValue("@FirstName", patient.FirstName);
            cmd.Parameters.AddWithValue("@LastName", patient.LastName);
            cmd.Parameters.AddWithValue("@Gender", patient.Gender);
            cmd.Parameters.AddWithValue("@DOB", patient.DOB.ToDateTime(TimeOnly.MinValue));
            cmd.Parameters.AddWithValue("@PhoneNumber", patient.PhoneNumber);
            cmd.Parameters.AddWithValue("@Email", (object?)patient.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Address", (object?)patient.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EmergencyContactName", (object?)patient.EmergencyName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EmergencyContactPhone",(object?)patient.EmergencyContactPhone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BloodGroup", patient.BloodGroup);
            cmd.Parameters.AddWithValue("@MaritalStatus", patient.MaritalStatus);
            cmd.Parameters.AddWithValue("@City", patient.City);
            cmd.Parameters.AddWithValue("@Country", patient.Country);
            cmd.Parameters.AddWithValue("@Nationality", patient.Nationality);
            cmd.Parameters.AddWithValue("@Status",patient.Status ?? "Active");

            return await cmd.ExecuteNonQueryAsync() > 0;
        }
        public async Task<bool> UpdatePatientAsync(string patientId, MdPostPatient patient)
        {
            const string sql = @"
                UPDATE Patients
                SET
                    FirstName=@FirstName,
                    LastName=@LastName,
                    Gender=@Gender,
                    DOB=@DOB,
                    PhoneNumber=@PhoneNumber,
                    Email=@Email,
                    Address=@Address,
                    EmergencyContactName=@EmergencyContactName,
                    EmergencyContactPhone=@EmergencyContactPhone,
                    BloodGroup=@BloodGroup,
                    MaritalStatus=@MaritalStatus,
                    City=@City,
                    Country=@Country,
                    Nationality=@Nationality,
                    Status=@Status,
                    UpdatedAt=GETDATE()
                WHERE Id=@Id;
            ";

            using var connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();

            using var cmd = new SqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@Id", patientId);
            cmd.Parameters.AddWithValue("@FirstName", patient.FirstName);
            cmd.Parameters.AddWithValue("@LastName", patient.LastName);
            cmd.Parameters.AddWithValue("@Gender", patient.Gender);
            cmd.Parameters.AddWithValue("@DOB", patient.DOB.ToDateTime(TimeOnly.MinValue));
            cmd.Parameters.AddWithValue("@PhoneNumber", patient.PhoneNumber);
            cmd.Parameters.AddWithValue("@Email", (object?)patient.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Address", (object?)patient.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EmergencyContactName", (object?)patient.EmergencyName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EmergencyContactPhone", (object?)patient.EmergencyContactPhone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BloodGroup", patient.BloodGroup);
            cmd.Parameters.AddWithValue("@MaritalStatus", patient.MaritalStatus);
            cmd.Parameters.AddWithValue("@City", patient.City);
            cmd.Parameters.AddWithValue("@Country", patient.Country);
            cmd.Parameters.AddWithValue("@Nationality", patient.Nationality);
            cmd.Parameters.AddWithValue("@Status", patient.Status ?? "Active");

            return await cmd.ExecuteNonQueryAsync() > 0;
        }
        public async Task<List<MdGetPatient>> GetPatientsAsync(string? search = null)
        {
            var patients = new List<MdGetPatient>();

            const string sql = @"
                SELECT
                    *
                FROM Patients
                WHERE
                (
                    @Search IS NULL
                    OR FirstName LIKE '%' + @Search + '%'
                    OR LastName LIKE '%' + @Search + '%'
                    OR (FirstName + ' ' + LastName) LIKE '%' + @Search + '%'
                )
                ORDER BY CreatedAt DESC;";

            using var connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Search",string.IsNullOrWhiteSpace(search) ? DBNull.Value : search.Trim());

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                patients.Add(MapPatient(reader));
            }

            return patients;
        }
        public async Task<MdGetPatient?> GetPatientAsync(string patientId)
        {
            const string sql = @"
                SELECT *
                FROM Patients
                WHERE Id = @Id;";

            using var connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Id", patientId);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapPatient(reader);
            }

            return null;
        }


        private static MdGetPatient MapPatient(SqlDataReader reader)
        {
            return new MdGetPatient
            {
                Id = reader["Id"].ToString(),
                FirstName = reader["FirstName"].ToString(),
                LastName = reader["LastName"].ToString(),
                Gender = reader["Gender"]?.ToString(),
                DOB = DateOnly.FromDateTime(Convert.ToDateTime(reader["DOB"])),
                PhoneNumber = reader["PhoneNumber"]?.ToString(),
                Email = reader["Email"]?.ToString(),
                Address = reader["Address"]?.ToString(),
                EmergencyName = reader["EmergencyContactName"]?.ToString(),
                EmergencyContactPhone = reader["EmergencyContactPhone"]?.ToString(),
                BloodGroup = reader["BloodGroup"]?.ToString(),
                MaritalStatus = reader["MaritalStatus"]?.ToString(),
                City = reader["City"]?.ToString(),
                Country = reader["Country"]?.ToString(),
                Nationality = reader["Nationality"]?.ToString(),
                Status = reader["Status"]?.ToString(),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
            };
        }
    }
}
