using WebAPI.DTO;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public static class DoctorMapper
    {
        public static Doctor ToDoctor(int id, DoctorREST dto)
        {
            return new Doctor
            {
                Id = id,
                Name = dto.Name,
                Age = dto.Age,
                Specialty = dto.Specialty,
            };
        }
        public static void UpdateDoctor(Doctor doctor, DoctorREST dto)
        {
           doctor.Name = dto.Name;
           doctor.Age = dto.Age;
           doctor.Specialty = dto.Specialty;
        }
    }
}
