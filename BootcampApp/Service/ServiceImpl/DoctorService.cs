using Model.DTO;
using Model.Models;
using Service.Mapper;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ServiceImpl
{
    public class DoctorService
    {
        private DoctorRepository _repo = new DoctorRepository();
        public async Task<Doctor> CreateAsync(DoctorREST doctorREST) 
        {
            var doctor = DoctorMapper.ToDoctor(doctorREST);
            await _repo.CreateAsync(doctor);
            return doctor;

        }

        public async Task<Doctor?> UpdateAsync(Guid doctorId, DoctorREST doctorREST)
        {
            var doctor = DoctorMapper.UpdateDoctor(doctorId, doctorREST);
            if(await _repo.UpdateAsync(doctorId, doctor))
            {
                return doctor;
            }
            return null;
        }

        public async Task<Doctor?> GetAsync(Guid doctorId)
        {
            return await _repo.GetAsync(doctorId);
        }

        public async Task<bool> DeleteAsync(Guid doctorId)
        {
            return await _repo.DeleteAsync(doctorId);
        }
    }
}
