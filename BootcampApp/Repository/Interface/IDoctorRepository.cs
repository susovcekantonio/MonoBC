using Model.Models;

namespace Repository.Interface
{
    public interface IDoctorRepository
    {
        Task CreateAsync(Doctor doctor);
        Task<bool> DeleteAsync(Guid doctorId);
        Task<Doctor?> GetAsync(Guid doctorId);
        Task<bool> UpdateAsync(Guid doctorId, Doctor doctor);
    }
}