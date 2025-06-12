using Model.DTO;
using Model.Models;

namespace Service.Interface
{
    public interface IDoctorService
    {
        Task<Doctor> CreateAsync(DoctorREST doctorREST);
        Task<bool> DeleteAsync(Guid doctorId);
        Task<Doctor?> GetAsync(Guid doctorId);
        Task<Doctor?> UpdateAsync(Guid doctorId, DoctorREST doctorREST);
    }
}