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
        public async Task<ActionResult<Doctor>> CreateAsync([FromBody]DoctorREST doctorREST)
        {
            if (doctorREST.Age <= 0)
            {
                return BadRequest("Age cannot be lower than 0");
            }
            var doctor = await _service.CreateAsync(doctorREST);    

            return Ok(doctor);
        }

        [HttpPut("update/{doctorId}")]
        public async Task<ActionResult<Doctor>> UpdateAsync(Guid doctorId, [FromBody] DoctorREST doctorREST)
        {
            if (doctorREST.Age <= 0)
            {
                return BadRequest("Age cannot be lower than 0");
            }
            var doctor = await _service.UpdateAsync(doctorId, doctorREST);
          
            if(doctor==null)
            {
                return NotFound("Doctor with this ID not found");
            }

            return Ok(doctor);
        }

        [HttpGet("get/{doctorId}")]
        public async Task<ActionResult<Doctor>> GetAsync(Guid doctorId)
        {
            var doctor = await _service.GetAsync(doctorId);

            if (doctor == null)
            {
                return NotFound("Doctor with this ID not found");
            }
            return Ok(doctor);
        }

        [HttpDelete("delete/{doctorId}")]
        public async Task<ActionResult> DeleteAsync(Guid doctorId)
        {
            if (await _service.DeleteAsync(doctorId))
            {
                return Ok("Doctor deleted");
            }
            return NotFound("Doctor with this ID not found");          
        }

    }
}
