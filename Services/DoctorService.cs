using HospitalManagementSystem.Interfaces;
using HospitalManagementSystem.Models.Doctor;
using HospitalManagementSystem.Repository;

namespace HospitalManagementSystem.Services
{
    public class DoctorService(IDoctorRepo repo) : IDoctor
    {
        public async Task<bool> AddDoctorAsync(DoctorPost doctor)
        {
            return doctor == null ? 
                throw new ArgumentNullException($"Invalid data: {nameof(doctor)}") 
                : 
                await repo.AddDoctorAsync(doctor);
        }
        public async Task<bool> UpdateDoctorAsync(string doctorId, DoctorPost doctor)
        {
            var existing = await repo.GetDoctorAsync(doctorId);
            if (existing == null) 
                return false;

            existing.PhoneNumber = doctor.PhoneNumber ?? existing.PhoneNumber;
            existing.Email = doctor.Email ?? existing.Email;
            existing.ConsultationFee = doctor.ConsultationFee;
            existing.FirstName = doctor.FirstName ?? existing.FirstName;
            existing.LastName = doctor.LastName ??  existing.LastName;
            existing.Gender = doctor.Gender ?? existing.Gender;
            existing.Specialization = doctor.Specialization ?? existing.Specialization;
            existing.DepartementId = doctor.DepartementId ?? existing.DepartementId;
            existing.LicenseNumber = doctor.LicenseNumber ?? existing.LicenseNumber;

            return await repo.UpdateDoctorAsync(existing);

        }
        public async Task<List<DoctorGet>> GetDoctorsAsync(string? search = null)
        {
            return await repo.GetDoctorsAsync(search);
        }
        public async Task<DoctorGet?> GetDoctorAsync(string doctorId)
        {
            return await repo.GetDoctorAsync(doctorId ?? throw new ArgumentNullException());
        }
        public async Task<bool> DeleteDoctorAsync(string doctorId)
        {
            return await repo.DeleteDoctorAsync(doctorId ?? throw new ArgumentNullException());
        }
    }
}
