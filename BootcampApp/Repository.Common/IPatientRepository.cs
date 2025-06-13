using Model.Models;

namespace Repository.Common
{
    public interface IPatientRepository
    {
        Task CreateAsync(Guid doctorId, Patient patient);
        Task<bool> DeleteAsync(Guid patientId);
        Task<Doctor> GetAllPatientsAsync(Guid doctorId);
        Task<Doctor?> GetDoctorWithPatientsPaginatedAsync(Guid doctorId, int page, int pageSize, string sort, string order);
        Task<bool> UpdateAsync(Guid doctorId, Guid patientId, Patient patient);
    }
}