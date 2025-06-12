using Model.DTO;
using Model.Models;

namespace Service.Interface
{
    public interface IMedicalRecordService
    {
        Task<MedicalRecord> CreateAsync(Guid patientId, MedicalRecordREST medicalRecordREST);
        Task<Patient?> GetPatientWithRecordAsync(Guid patientId);
        Task<MedicalRecord> UpdateAsync(Guid patientId, Guid recordId, MedicalRecordREST medicalRecordREST);
    }
}