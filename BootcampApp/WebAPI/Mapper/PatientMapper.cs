using WebAPI.DTO;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public static class PatientMapper
    {
        public static Patient ToPatient(Guid doctorId, PatientREST dto)
        {
            return new Patient
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Age = dto.Age,
                Condition = dto.Condition,
                DoctorId = doctorId,
            };
        }
    }
}
