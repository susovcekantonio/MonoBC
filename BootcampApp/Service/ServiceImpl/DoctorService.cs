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
        public Doctor Create(DoctorREST doctorREST) 
        {
            var doctor = DoctorMapper.ToDoctor(doctorREST);
            doctor.Id = Guid.NewGuid();
            _repo.Create(doctor);
            return doctor;

        }

        public Doctor? Update(Guid doctorId, DoctorREST doctorREST)
        {
            var doctor = DoctorMapper.ToDoctor(doctorREST);
            doctor.Id = doctorId;
            if(_repo.Update(doctorId, doctor))
            {
                return doctor;
            }
            return null;
        }

        public Doctor? Get(Guid doctorId)
        {
            return _repo.Get(doctorId);
        }

        public bool Delete(Guid doctorId)
        {
            return _repo.Delete(doctorId);
        }
    }
}
