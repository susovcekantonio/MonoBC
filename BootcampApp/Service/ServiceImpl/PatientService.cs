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
    public class PatientService
    {
        private PatientRepository _repo = new PatientRepository();

        public Patient Create(Guid doctorId, PatientREST patientREST)
        {
            var patient = PatientMapper.ToPatient(doctorId, patientREST);
            patient.Id = Guid.NewGuid();
            _repo.Create(doctorId, patient);
            return patient;
        }

        public Patient? Update(Guid doctorId, Guid patientId, PatientREST patientREST)
        {
            var patient = PatientMapper.ToPatient(doctorId, patientREST);
            patient.Id = patientId;
            if (_repo.Update(doctorId, patientId, patient))
            {
                return patient;
            }
            return null;
        }

        public Doctor? GetAllPatients(Guid doctorId)
        {
            return _repo.GetAllPatients(doctorId);
        }

        public bool Delete(Guid patientId)
        {
            return _repo.Delete(patientId);
        }
    }
}
