using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Mapper;
using Service.ServiceImpl;
using Model.Models;
using Model.DTO;
using Npgsql;
using System.Numerics;


namespace WebAPI.Controllers
{
    [ApiController]
    [Route("doctor")]
    public class DoctorController : Controller
    {
        private DoctorService _service = new DoctorService();
         
        [HttpPost("create")]
        public ActionResult<Doctor> Create([FromBody]DoctorREST doctorREST)
        {
            if (doctorREST.Age <= 0)
            {
                return BadRequest("Age cannot be lower than 0");
            }
            var doctor = _service.Create(doctorREST);    

            return Ok(doctor);
        }

        [HttpPut("update/{doctorId}")]
        public ActionResult<Doctor> Update(Guid doctorId, [FromBody] DoctorREST doctorREST)
        {
            if (doctorREST.Age <= 0)
            {
                return BadRequest("Age cannot be lower than 0");
            }
            var doctor = _service.Update(doctorId, doctorREST);
          
            if(doctor==null)
            {
                return NotFound("Doctor with this ID not found");
            }

            return Ok(doctor);
        }

        [HttpGet("get/{doctorId}")]
        public ActionResult<Doctor> Get(Guid doctorId)
        {
            var doctor = _service.Get(doctorId);

            if (doctor == null)
            {
                return NotFound("Doctor with this ID not found");
            }
            return Ok(doctor);
        }

        [HttpDelete("delete/{doctorId}")]
        public ActionResult Delete(Guid doctorId)
        {
            if (_service.Delete(doctorId))
            {
                return Ok("Doctor deleted");
            }
            return NotFound("Doctor with this ID not found");          
        }

    }
}
