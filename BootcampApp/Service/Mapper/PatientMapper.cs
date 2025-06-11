using Model.DTO;
using Model.Models;

namespace Service.Mapper
{
    public static class PatientMapper
    {
        public static Patient ToPatient(Guid doctorId, PatientREST patientREST)
        {
            return new Patient
            {
                Id = Guid.NewGuid(),
                Name = patientREST.Name,
                Age = patientREST.Age,
                Condition = patientREST.Condition,
                DoctorId = doctorId,
            };
        }

        public static Patient UpdatePatient(Guid doctorId, Guid patientId, PatientREST patientREST)
        {
            return new Patient
            {
                Id = patientId,
                Name = patientREST.Name,
                Age = patientREST.Age,
                Condition = patientREST.Condition,
                DoctorId = doctorId,
            };
        }
    }
}
