using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.DTO;
using WebAPI.Mapper;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("doctor")]
    public class DoctorController : Controller
    {
        private static List<Doctor> doctors= new List<Doctor>();
        private static int doctorId = 0;
         
        [HttpPost("create")]
        public ActionResult<Doctor> Create([FromBody]DoctorREST dto)
        {
            if (dto.Age <= 0)
            {
                return BadRequest("Age cannot be lower than 0");
            }
            Doctor doctor = DoctorMapper.ToDoctor(doctorId, dto);
            doctors.Add(doctor);
            doctorId++;

            return Ok(doctor);
        }

        [HttpPut("update/{id}")]
        public ActionResult<Doctor> Update(int id, [FromBody] DoctorREST dto)
        {
            if (id >= doctors.Count || id<0)
            {
                return NotFound("Doctor not found");
            }
            if (dto.Age <= 0)
            {
                return BadRequest("Age cannot be lower than 0");
            }
            Doctor doctor = doctors[id];
            DoctorMapper.UpdateDoctor(doctor, dto);

            return Ok(doctor);
        }

        [HttpGet("get-all")]
        public ActionResult<IEnumerable<Doctor>> GetAll()
        {
            return Ok(doctors);
        }

        [HttpDelete("delete/{id}")]
        public ActionResult Delete(int id)
        {
            if (id >= doctors.Count || id < 0)
            {
                return NotFound("Doctor not found");
            }
            doctors.Remove(doctors[id]);

            return Ok("Doctor deleted");
        }

    }
}
