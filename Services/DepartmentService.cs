using HospitalManagementSystem.Models.Department;
using HospitalManagementSystem.Repository;

namespace HospitalManagementSystem.Services
{
    public interface IDepartment
    {
        Task<bool> CreateDepartment(DepartmentPost model);
        Task<bool> UpdateDepartment(string id, DepartmentPost model);
        Task<DepartmentGet?> GetDepartment(string id);
        Task<List<DepartmentGet>> GetDepartments(string? search = null);
        Task<bool> MapDepartmentHead(MapDepToDoc model);
        Task<(bool, string)> DeleteDepartment(string id);
    }
    public class DepartmentService(IDepartmentRepo repo) : IDepartment
    {
        public async Task<bool> CreateDepartment(DepartmentPost model)
        {
            if(model == null)
            {
                throw new ArgumentNullException($"Invalida data: {nameof(model)}");
            }
            return await repo.CreateDepartment(model);
        }
        public async Task<bool> UpdateDepartment(string id, DepartmentPost model)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("Id is required");
            }
            if(model == null)
            {
                throw new ArgumentNullException($"Invalida data: {nameof(model)}");
            }
            return await repo.UpdateDepartment(id, model);
        }
        public async Task<bool> MapDepartmentHead(MapDepToDoc model)
        {
            if (model == null)
            {
                throw new ArgumentNullException($"Invalida data: {nameof(model)}");
            }
            return await repo.MapDepartmentHead(model);
        }
        public async Task<(bool, string)> DeleteDepartment(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("Id is required");
            }

            return await repo.DeleteDepartment(id);
        }
        public async Task<DepartmentGet?> GetDepartment(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("Id is required");
            }
            return await repo.GetDepartment(id);
        }
        public async Task<List<DepartmentGet>> GetDepartments(string? search)
        {
            return await repo.GetDepartments(search);
        }
    }
}
