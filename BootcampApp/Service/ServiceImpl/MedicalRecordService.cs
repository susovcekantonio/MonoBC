using Model.DTO;
using Model.Models;
using Repository;
using Service.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ServiceImpl
{
    public class MedicalRecordService
    {
        private MedicalRecordRepository _repo = new MedicalRecordRepository();

        public async Task<MedicalRecord> CreateAsync(Guid patientId, MedicalRecordREST medicalRecordREST)
        {
            var record = MedicalRecordMapper.ToMedicalRecord(patientId, medicalRecordREST);
            await _repo.CreateAsync(patientId, record);
            return record;
        }

        public async Task<MedicalRecord> UpdateAsync(Guid patientId, Guid recordId, MedicalRecordREST medicalRecordREST)
        {
            var record = MedicalRecordMapper.UpdateMedicalRecord(patientId, recordId, medicalRecordREST);
            await _repo.UpdateAsync(patientId, record);
            return record;
        }

        public async Task<Patient?> GetPatientWithRecordAsync(Guid patientId)
        {
            return await _repo.GetPatientWithRecordAsync(patientId);
        }
    }
}
