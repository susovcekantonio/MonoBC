using Model.DTO;
using Model.Models;
using Repository;
using Repository.Common;
using Service.Common;
using Service.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ServiceImpl
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repo;

        public PatientService(IPatientRepository repo)
        {
            this._repo = repo;
        }

        public async Task<Patient> CreateAsync(Guid doctorId, PatientREST patientREST)
        {
            var patient = PatientMapper.ToPatient(doctorId, patientREST);
            await _repo.CreateAsync(doctorId, patient);
            return patient;
        }

        public async Task<Patient?> UpdateAsync(Guid doctorId, Guid patientId, PatientREST patientREST)
        {
            var patient = PatientMapper.UpdatePatient(doctorId, patientId, patientREST);
            if (await _repo.UpdateAsync(doctorId, patientId, patient))
            {
                return patient;
            }
            return null;
        }

        public async Task<Doctor?> GetAllPatientsAsync(Guid doctorId)
        {
            return await _repo.GetAllPatientsAsync(doctorId);
        }

        public async Task<Doctor?> GetDoctorWithPatientsPaginatedAsync(Guid doctorId, int page, int pageSize, string sort, string order)
        {
            return await _repo.GetDoctorWithPatientsPaginatedAsync(doctorId, page, pageSize, sort, order);
        }

        public async Task<bool> DeleteAsync(Guid patientId)
        {
            return await _repo.DeleteAsync(patientId);
        }
    }
}
