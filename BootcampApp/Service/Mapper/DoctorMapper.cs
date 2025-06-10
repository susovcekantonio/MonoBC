using Model.DTO;
using Model.Models;

namespace Service.Mapper
{
    public static class DoctorMapper
    {
        public static Doctor ToDoctor(DoctorREST doctorREST)
        {
            return new Doctor
            {
                Id = Guid.NewGuid(),
                Name = doctorREST.Name,
                Age = doctorREST.Age,
                Specialty = doctorREST.Specialty,
                Patients = new List<Patient>()
            };
        }
    }
}
