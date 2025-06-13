using Model.Models;

namespace Repository.Common
{
    public interface IMedicalRecordRepository
    {
        Task CreateAsync(Guid patientId, MedicalRecord record);
        Task<Patient?> GetPatientWithRecordAsync(Guid patientId);
        Task<bool> UpdateAsync(Guid patientId, MedicalRecord record);
    }
}