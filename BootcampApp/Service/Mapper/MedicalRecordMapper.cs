using Model.DTO;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Mapper
{
    public static class MedicalRecordMapper
    {
        public static MedicalRecord ToMedicalRecord(Guid patientId, MedicalRecordREST medicalRecordREST)
        {
            return new MedicalRecord
            {
                Id = Guid.NewGuid(),
                Treatment = medicalRecordREST.Treatment,
                PatientId = patientId,
            };
        }

        public static MedicalRecord UpdateMedicalRecord(Guid patientId, Guid recordId, MedicalRecordREST medicalRecordREST)
        {
            return new MedicalRecord
            {
                Id = recordId,
                Treatment = medicalRecordREST.Treatment,
                PatientId = patientId,
            };
        }
    }
}
