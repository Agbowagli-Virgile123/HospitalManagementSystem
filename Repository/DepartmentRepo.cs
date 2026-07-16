using HospitalManagementSystem.Models.Department;
using HospitalManagementSystem.Models.Doctor;
using Microsoft.Data.SqlClient;

namespace HospitalManagementSystem.Repository
{
    public interface IDepartmentRepo
    {
        Task<bool> CreateDepartment(DepartmentPost model);
        Task<bool> UpdateDepartment(string id, DepartmentPost model);
        Task<DepartmentGet?> GetDepartment(string id);
        Task<List<DepartmentGet>> GetDepartments(string? search);
        Task<bool> MapDepartmentHead(MapDepToDoc model);
        Task<(bool,string)> DeleteDepartment(string id);
    }
    public class DepartmentRepo(IConfiguration config) : IDepartmentRepo
    {
        private readonly string _connectionString = config.GetConnectionString("DefaultConnection")!;

        // POST/PUT
        public async Task<bool> CreateDepartment(DepartmentPost model)
        {
            const string sql = @"
                INSERT INTO Department
                (
                    Id,
                    DeptName,
                    DeptDescription,
                    DeptLocation,
                    DeptContact,
                    DeptStatus
                )
                VALUES
                (
                    @Id,
                    @DeptName,
                    @DeptDescription,
                    @DeptLocation,
                    @DeptContact,
                    @DeptStatus
                );
            ";

            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            using SqlCommand cmd = new(sql, conn);

            cmd.Parameters.AddWithValue("@Id", Guid.NewGuid().ToString());
            cmd.Parameters.AddWithValue("@DeptName", model.DeptName);
            cmd.Parameters.AddWithValue("@DeptDescription", model.DeptDescription);
            cmd.Parameters.AddWithValue("@DeptLocation", model.DeptLocation);
            cmd.Parameters.AddWithValue("@DeptContact", model.DeptContact);
            cmd.Parameters.AddWithValue("@DeptStatus", model.DeptStatus);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }
        public async Task<bool> UpdateDepartment(string id, DepartmentPost model)
        {
            const string sql = @"
                UPDATE Department
                SET
                    DeptName = @DeptName,
                    DeptDescription = @DeptDescription,
                    DeptLocation = @DeptLocation,
                    DeptContact = @DeptContact,
                    DeptStatus = @DeptStatus,
                    UpdatedDate = GETDATE()
                WHERE Id = @Id;
            ";

            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            using SqlCommand cmd = new(sql, conn);

            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@DeptName", model.DeptName);
            cmd.Parameters.AddWithValue("@DeptDescription", model.DeptDescription);
            cmd.Parameters.AddWithValue("@DeptLocation", model.DeptLocation);
            cmd.Parameters.AddWithValue("@DeptContact", model.DeptContact);
            cmd.Parameters.AddWithValue("@DeptStatus", model.DeptStatus);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }
        public async Task<bool> MapDepartmentHead(MapDepToDoc model)
        {
            const string sql = @"
            UPDATE Department
                SET
                    HeadDoctorId = @DoctorId,
                    UpdatedDate = GETDATE()
                WHERE Id = @DepartmentId;
            ";

            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            using SqlCommand cmd = new(sql, conn);

            cmd.Parameters.AddWithValue("@DepartmentId", model.DeptId);
            cmd.Parameters.AddWithValue("@DoctorId", model.DocId);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        // GETS
        public async Task<DepartmentGet?> GetDepartment(string id)
        {
            const string sql = @"
                SELECT
                    dep.*,
                    doc.Id DoctorId,
                    doc.FirstName,
                    doc.LastName,
                    doc.Specialization,
                    doc.PhoneNumber,
                    doc.Email,
                FROM Department dep
                LEFT JOIN Doctors doc
                    ON dep.HeadDoctorId = doc.Id
                WHERE dep.Id = @Id;
            ";

            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            using SqlCommand cmd = new(sql, conn);

            cmd.Parameters.AddWithValue("@Id", id);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return new DepartmentGet
            {
                Id = reader["Id"].ToString()!,
                DeptName = reader["DeptName"].ToString()!,
                DeptDescription = reader["DeptDescription"].ToString()!,
                DeptLocation = reader["DeptLocation"].ToString()!,
                DeptContact = reader["DeptContact"].ToString()!,
                DeptStatus = reader["DeptStatus"].ToString()!,
                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                UpdatedDate = reader["UpdatedDate"] == DBNull.Value
                    ? default
                    : Convert.ToDateTime(reader["UpdatedDate"]),
                HeadDoctor = reader["DoctorId"] == DBNull.Value
                    ? null
                    : new DoctorGet
                    {
                        Id = reader["DoctorId"].ToString()!,
                        FirstName = reader["FirstName"].ToString()!,
                        LastName = reader["LastName"].ToString()!,
                        Specialization = reader["Specialization"].ToString()!,
                        PhoneNumber = reader["PhoneNumber"].ToString()!,
                        Email = reader["Email"].ToString()!,
                    }
            };
        }
        public async Task<List<DepartmentGet>> GetDepartments(string? search)
        {
            const string sql = @"
                SELECT
                    dep.*,
                    doc.Id DoctorId,
                    doc.FirstName,
                    doc.LastName,
                    doc.Specialization,
                    doc.PhoneNumber,
                    doc.Email
                FROM Department dep
                LEFT JOIN Doctors doc
                    ON dep.HeadDoctorId = doc.Id
                WHERE
                    @Search IS NULL
                    OR dep.DeptName LIKE '%' + @Search + '%'
                ORDER BY dep.DeptName;
            ";

            List<DepartmentGet> departments = new();

            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            using SqlCommand cmd = new(sql, conn);

            cmd.Parameters.AddWithValue("@Search", (object?)search ?? DBNull.Value);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                departments.Add(new DepartmentGet
                {
                    Id = reader["Id"].ToString()!,
                    DeptName = reader["DeptName"].ToString()!,
                    DeptDescription = reader["DeptDescription"].ToString()!,
                    DeptLocation = reader["DeptLocation"].ToString()!,
                    DeptContact = reader["DeptContact"].ToString()!,
                    DeptStatus = reader["DeptStatus"].ToString()!,
                    CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                    UpdatedDate = reader["UpdatedDate"] == DBNull.Value
                        ? default
                        : Convert.ToDateTime(reader["UpdatedDate"]),
                    HeadDoctor = reader["DoctorId"] == DBNull.Value
                        ? null
                        : new DoctorGet
                        {
                            Id = reader["DoctorId"].ToString()!,
                            FirstName = reader["FirstName"].ToString()!,
                            LastName = reader["LastName"].ToString()!,
                            Specialization = reader["Specialization"].ToString()!,
                            PhoneNumber = reader["PhoneNumber"].ToString()!,
                            Email = reader["Email"].ToString()!,
                        }
                });
            }

            return departments;
        }

        // DELETE
        public async Task<(bool, string)> DeleteDepartment(string id)
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            // Check if the department has doctors
            const string checkSql = @"
                SELECT COUNT(*)
                FROM Doctors
                WHERE DepartmentId = @Id;
            ";

            using SqlCommand checkCmd = new(checkSql, conn);
            checkCmd.Parameters.AddWithValue("@Id", id);

            int doctorCount = Convert.ToInt32(await checkCmd.ExecuteScalarAsync());

            if (doctorCount > 0)
            {
                return (false, "This department cannot be deleted because it is currently assigned to one or more doctors.");
            }

            // Delete department
            const string deleteSql = @"
                DELETE FROM Department
                WHERE Id = @Id;
            ";

            using SqlCommand deleteCmd = new(deleteSql, conn);
            deleteCmd.Parameters.AddWithValue("@Id", id);

            int rows = await deleteCmd.ExecuteNonQueryAsync();

            if (rows == 0)
            {
                return (false, "Department not found.");
            }

            return (true, "Department deleted successfully.");
        }

    }
}
