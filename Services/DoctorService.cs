using HospitalManagementSystem.Interfaces;
using HospitalManagementSystem.Models.Doctor;
using HospitalManagementSystem.Repository;

namespace HospitalManagementSystem.Services
{
    public class DoctorService(IDoctorRepo repo) : IDoctor
    {
        public async Task<bool> AddDoctorAsync(Doctor doctor)
        {
            return doctor == null ? throw new ArgumentNullException($"Invalid data: {nameof(doctor)}") : await repo.AddDoctorAsync(doctor);
        }
        public async Task<bool> UpdateDoctorAsync(Doctor doctor)
        {
            var existing = await repo.GetDoctorAsync(doctor.Id);
            if (existing != null) 
                return false;

            existing.PhoneNumber = doctor.PhoneNumber ?? existing.PhoneNumber;
            existing.Email = doctor.Email ?? existing.Email;
            existing.ConsultationFee = doctor.ConsultationFee;
            existing.FirstName = doctor.FirstName ?? existing.FirstName;
            existing.LastName = doctor.LastName ??  existing.LastName;
            existing.Gender = doctor.Gender ?? existing.Gender;
            existing.Specialization = doctor.Specialization;

            return await repo.UpdateDoctorAsync(existing);

        }
        public async Task<List<Doctor>> GetDoctorsAsync(string? search = null)
        {
            return await repo.GetDoctorsAsync(search);
        }
        public async Task<Doctor?> GetDoctorAsync(string doctorId)
        {
            return await repo.GetDoctorAsync(doctorId ?? throw new ArgumentNullException());
        }
        public async Task<bool> DeleteDoctorAsync(string doctorId)
        {
            return await repo.DeleteDoctorAsync(doctorId ?? throw new ArgumentNullException());
        }
    }
}
