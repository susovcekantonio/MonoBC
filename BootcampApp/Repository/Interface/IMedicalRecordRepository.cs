using Model.Models;

namespace Repository.Interface
{
    public interface IMedicalRecordRepository
    {
        Task CreateAsync(Guid patientId, MedicalRecord record);
        Task<Patient?> GetPatientWithRecordAsync(Guid patientId);
        Task<bool> UpdateAsync(Guid patientId, MedicalRecord record);
    }
}