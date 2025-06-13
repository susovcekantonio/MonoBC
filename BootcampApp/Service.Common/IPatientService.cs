using Model.DTO;
using Model.Models;

namespace Service.Common
{
    public interface IPatientService
    {
        Task<Patient> CreateAsync(Guid doctorId, PatientREST patientREST);
        Task<bool> DeleteAsync(Guid patientId);
        Task<Doctor?> GetAllPatientsAsync(Guid doctorId);
        Task<Doctor?> GetDoctorWithPatientsPaginatedAsync(Guid doctorId, int page, int pageSize, string sort, string order);
        Task<Patient?> UpdateAsync(Guid doctorId, Guid patientId, PatientREST patientREST);
    }
}