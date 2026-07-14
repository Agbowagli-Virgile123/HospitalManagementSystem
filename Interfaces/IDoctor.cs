using HospitalManagementSystem.Models.Doctor;

namespace HospitalManagementSystem.Interfaces
{
    public interface IDoctor
    {
        Task<bool> AddDoctorAsync(DoctorPost doctor);
        Task<bool> UpdateDoctorAsync(string doctorId,DoctorPost doctor);
        Task<List<DoctorGet>> GetDoctorsAsync(string? search = null);
        Task<DoctorGet?> GetDoctorAsync(string doctorId);
        Task<bool> DeleteDoctorAsync(string doctorId);
    }
}
