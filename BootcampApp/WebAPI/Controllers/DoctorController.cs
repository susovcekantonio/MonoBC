using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.DTO;
using WebAPI.Mapper;
using Npgsql;
using System.Numerics;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("doctor")]
    public class DoctorController : Controller
    {
        private readonly string _connection = "Host=localhost;Port=5433;Username=postgres;Password=postgres;Database=PatientRecord";
         
        [HttpPost("create")]
        public ActionResult<Doctor> Create([FromBody]DoctorREST dto)
        {
            if (dto.Age <= 0)
            {
                return BadRequest("Age cannot be lower than 0");
            }
            Doctor doctor = DoctorMapper.ToDoctor(dto);
            
            using var conn = new NpgsqlConnection(_connection);
            conn.Open();

            using var cmd = new NpgsqlCommand("INSERT INTO Doctor values (@doctor_id, @doctor_name, @age, @specialty)", conn);
            cmd.Parameters.AddWithValue("doctor_id", doctor.Id);
            cmd.Parameters.AddWithValue("doctor_name", doctor.Name);
            cmd.Parameters.AddWithValue("age", doctor.Age);
            cmd.Parameters.AddWithValue("specialty", doctor.Specialty);

            cmd.ExecuteNonQuery();

            return Ok(doctor);
        }

        [HttpPut("update/{id}")]
        public ActionResult<Doctor> Update(Guid id, [FromBody] DoctorREST dto)
        {
            if (dto.Age <= 0)
            {
                return BadRequest("Age cannot be lower than 0");
            }
            Doctor doctor = DoctorMapper.ToDoctor(dto);

            using var conn = new NpgsqlConnection(_connection);
            conn.Open();

            using var cmd = new NpgsqlCommand("UPDATE Doctor SET doctor_name=@doctor_name, age=@age, specialty=@specialty WHERE doctor_id=@doctor_id", conn);
            cmd.Parameters.AddWithValue("doctor_id", id);
            cmd.Parameters.AddWithValue("doctor_name", doctor.Name);
            cmd.Parameters.AddWithValue("age", doctor.Age);
            cmd.Parameters.AddWithValue("specialty", doctor.Specialty);

            int rowsAffected = cmd.ExecuteNonQuery();

            if(rowsAffected == 0)
            {
                return NotFound("Doctor with this ID not found");
            }           

            return Ok(doctor);
        }

        [HttpGet("get/{id}")]
        public ActionResult<Doctor> GetAll(Guid id)
        {
            using var conn = new NpgsqlConnection(_connection);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM Doctor WHERE doctor_id=@doctor_id", conn);
            cmd.Parameters.AddWithValue("doctor_id", id);


            using (var rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    var doctor = new Doctor
                    {
                        Id = rdr.GetGuid(0),
                        Name = rdr.GetString(1),
                        Age = rdr.GetInt32(2),
                        Specialty = rdr.GetString(3)
                    };
                    return Ok(doctor);
                }            
            }
            return NotFound("Doctor with this ID not found");
        }

        [HttpDelete("delete/{id}")]
        public ActionResult Delete(Guid id)
        {
            using var conn = new NpgsqlConnection(_connection);
            conn.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM Doctor WHERE doctor_id=@doctor_id", conn);
            cmd.Parameters.AddWithValue("doctor_id", id);

            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                return NotFound("Doctor with this ID not found");
            }

            return Ok("Doctor deleted");
        }

    }
}
