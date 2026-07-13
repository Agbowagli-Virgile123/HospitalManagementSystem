using HospitalManagementSystem.Interfaces;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Models.Auth;
using Microsoft.Data.SqlClient;

namespace HospitalManagementSystem.Services
{
    public class AuthServices : IAuth
    {
        private readonly string _connectionString;

        public AuthServices(IConfiguration connectionString)
        {
            _connectionString = connectionString.GetConnectionString("DefaultConnection")!;
        }

        public async Task<(MdResponse response, MdUserInfo user)> Login(MdLogin req)
        {
            MdUserInfo user = new();

            if (string.IsNullOrEmpty(req.AcccessCode) || string.IsNullOrEmpty(req.Password))
            {
                return (new MdResponse { ResponseCode = 0, ResponseMessage = "Access code and password are required." }, user);
            }

            try
            {
                using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM Staff WHERE Id = @AccessCode AND password = @Password";
                
                using(SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AccessCode", req.AcccessCode);
                    command.Parameters.AddWithValue("@Password", req.Password);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = new MdUserInfo
                            {
                                Id = reader["Id"].ToString(),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Username = reader["Username"].ToString(),
                                Email = reader["Email"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Position = reader["Position"].ToString()
                            };
                        }
                    }
                }

             
                if (!string.IsNullOrEmpty(user.Id))
                {
                    return (new MdResponse { ResponseCode = 1, ResponseMessage = "Login successful." }, user);
                }
                else
                {
                    return (new MdResponse { ResponseCode = 0, ResponseMessage = "Invalid access code or password." }, user)!;
                }
            }

            }
            catch (Exception ex)
            {
                return (new MdResponse { ResponseCode = 0, ResponseMessage = $"An error occurred: {ex.Message}" }, user);
            }

        }
    }
}
