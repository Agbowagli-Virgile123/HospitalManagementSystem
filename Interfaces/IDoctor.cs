using HospitalManagementSystem.Models.Doctor;

namespace HospitalManagementSystem.Interfaces
{
    public interface IDoctor
    {
        Task<bool> AddDoctorAsync(Doctor doctor);
        Task<bool> UpdateDoctorAsync(Doctor doctor);
        Task<List<Doctor>> GetDoctorsAsync(string? search = null);
        Task<Doctor?> GetDoctorAsync(string doctorId);
        Task<bool> DeleteDoctorAsync(string doctorId);
    }
}
