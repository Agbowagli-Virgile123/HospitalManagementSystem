using HospitalManagementSystem.Models;
using HospitalManagementSystem.Models.Auth;

namespace HospitalManagementSystem.Interfaces
{
    public interface IAuth
    {
        Task<(MdResponse response, MdUserInfo user)> Login(MdLogin req);
    }
}
