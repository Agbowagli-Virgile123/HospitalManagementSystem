using HospitalManagementSystem.Models.Patients;
using HospitalManagementSystem.Repository;

namespace HospitalManagementSystem.Services
{
    public interface IPatient
    {

    }
    public class PatientService(IPatientRepository repo) : IPatient
    {
        public async Task<bool> CreatePatientAsync(MdPostPatient patient)
        {
            var requiredFields = new[]
            {
                nameof(patient.FirstName),
                nameof(patient.LastName),
                nameof(patient.Gender),
                nameof(patient.PhoneNumber),
                nameof(patient.MaritalStatus),
                nameof(patient.Nationality)
            };
            var result = ModelValidator.Validate(patient, requiredFields, true);
            if (!result.IsValid) return false;

            return await repo.CreatePatientAsync(patient);
        }
        public async Task<bool> UpdatePatientAsync(string patientId, MdPostPatient patient)
        {
            if (string.IsNullOrEmpty(patientId)) return false;

            MdGetPatient? existing = await repo.GetPatientAsync(patientId);

            if(existing == null) return false;

            patient.FirstName = patient.FirstName ?? existing.FirstName;
            patient.LastName = patient.LastName ?? existing.LastName;
            patient.Gender = patient.Gender ?? existing.Gender;
            patient.PhoneNumber = patient.PhoneNumber ?? existing.PhoneNumber;
            patient.Email = patient.Email ?? existing.Email;
            patient.Address = patient.Address ?? existing.Address;
            patient.Status = patient.Status ?? existing.Status;
            patient.EmergencyName = patient.EmergencyName ?? existing.EmergencyName;
            patient.EmergencyContactPhone = patient.EmergencyContactPhone ?? existing.EmergencyContactPhone;
            patient.BloodGroup = patient.BloodGroup ?? existing.BloodGroup;
            patient.MaritalStatus = patient.MaritalStatus ?? existing.MaritalStatus;
            patient.City = patient.City ?? existing.City;
            patient.Country = patient.Country ?? existing.Country;
            patient.Nationality = patient.Nationality ?? existing.Nationality;
            patient.AllergyDescription = patient.AllergyDescription ?? existing.AllergyDescription;
            patient.DOB = patient.DOB == default ? existing.DOB : patient.DOB;

            return await repo.UpdatePatientAsync(patientId, patient);
        }
        public async Task<List<MdGetPatient>> GetPatientsAsync(string? search = null)
            => await repo.GetPatientsAsync(search);
        public async Task<MdGetPatient?> GetPatientAsync(string patientId)
            => await repo.GetPatientAsync(patientId);
    }
}
