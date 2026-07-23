using HospitalManagementSystem.Models.Doctor;

namespace HospitalManagementSystem.Models.Department
{
    public class DepartmentPost
    {
        public string DeptName { get; set; } = string.Empty;
        public string DeptDescription { get; set; } = string.Empty;
        public string DeptLocation{ get; set; } = string.Empty;
        public string DeptContact{ get; set; } = string.Empty;
        public string DeptStatus{ get; set; } = string.Empty;
    }

    public class DepartmentGet : DepartmentPost
    {
        public string Id { get; set; } = string.Empty;
        public DoctorGet HeadDoctor { get; set; } = new DoctorGet();
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class MapDepToDoc
    {
        public string DeptId {  get; set; } = string.Empty;
        public string DocId {  get; set; } = string.Empty;
    }
}
