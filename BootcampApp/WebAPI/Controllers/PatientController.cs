using Microsoft.AspNetCore.Mvc;
using Npgsql;
using WebAPI.DTO;
using WebAPI.Mapper;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("doctor/{doctorId}/patient")]
    public class PatientController : Controller
    {
        private readonly string _connection = "Host=localhost;Port=5433;Username=postgres;Password=postgres;Database=PatientRecord";

        [HttpPost("create")]
        public ActionResult<Patient> Create(Guid doctorId, [FromBody] PatientREST dto)
        {
            if (dto.Age <= 0)
            {
                return BadRequest("Age cannot be lower than 0");
            }
            Patient patient = PatientMapper.ToPatient(doctorId, dto);

            using var conn = new NpgsqlConnection(_connection);
            conn.Open();

            using var cmd = new NpgsqlCommand("INSERT INTO Patient values (@patient_id, @patient_name, @age, @condition, @doctor_id)", conn);
            cmd.Parameters.AddWithValue("patient_id", patient.Id);
            cmd.Parameters.AddWithValue("patient_name", patient.Name);
            cmd.Parameters.AddWithValue("age", patient.Age);
            cmd.Parameters.AddWithValue("condition", patient.Condition);
            cmd.Parameters.AddWithValue("doctor_id", doctorId);

            cmd.ExecuteNonQuery();

            return Ok(patient);
        }

        [HttpPut("update/{patientId}")]
        public ActionResult<Doctor> Update(Guid doctorId, Guid patientId, [FromBody] PatientREST dto)
        {
            if (dto.Age <= 0)
            {
                return BadRequest("Age cannot be lower than 0");
            }
            Patient patient = PatientMapper.ToPatient(doctorId, dto);

            using var conn = new NpgsqlConnection(_connection);
            conn.Open();

            using var cmd = new NpgsqlCommand("UPDATE Patient SET patient_name=@patient_name, age=@age, condition=@condition WHERE patient_id=@patient_id", conn);
            cmd.Parameters.AddWithValue("patient_id", patientId);
            cmd.Parameters.AddWithValue("patient_name", patient.Name);
            cmd.Parameters.AddWithValue("age", patient.Age);
            cmd.Parameters.AddWithValue("condition", patient.Condition);
            cmd.Parameters.AddWithValue("doctor_id", doctorId);

            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                return NotFound("Patient with this ID not found");
            }

            return Ok(patient);
        }

        [HttpGet("get-patients")]
        public ActionResult<IEnumerable<Patient>> GetAllPatients(Guid doctorId)
        {
            List<Patient> patients = new List<Patient>();

            using var conn = new NpgsqlConnection(_connection);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT p.patient_id, p.patient_name, p.age, p.condition, d.doctor_id, d.doctor_name FROM Patient p INNER JOIN Doctor d on p.doctor_id = d.doctor_id ", conn);

            cmd.Parameters.AddWithValue("doctor_id", doctorId);


            using (var rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    patients.Add(new Patient
                    {
                        Id = rdr.GetGuid(0),
                        Name = rdr.GetString(1),
                        Age = rdr.GetInt32(2),
                        Condition = rdr.GetString(3),
                        DoctorId = doctorId
                    });


                }
                if (patients.Count == 0) return NotFound("This doctor has no patients");
                return Ok(patients);
            }
        }

        [HttpDelete("delete/{patientId}")]
        public ActionResult Delete(Guid patientId)
        {
            using var conn = new NpgsqlConnection(_connection);
            conn.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM Patient WHERE patient_id=@patient_id", conn);
            cmd.Parameters.AddWithValue("patient_id", patientId);

            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                return NotFound("Patient with this ID not found");
            }

            return Ok("Patient deleted");
        }
    }
}
