using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Model.Models;
using Service.Mapper;
using Model.DTO;
using Service.ServiceImpl;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("doctor/{doctorId}/patient")]
    public class PatientController : Controller
    {
        private PatientService _service = new PatientService();

        [HttpPost("create")]
        public async Task<ActionResult<Patient>> CreateAsync(Guid doctorId, [FromBody] PatientREST patientREST)
        {
            if (patientREST.Age <= 0)
            {
                return BadRequest("Age cannot be lower than 0");
            }
            var patient = await _service.CreateAsync(doctorId, patientREST);
            return Ok(patient);
        }

        [HttpPut("update/{patientId}")]
        public async Task<ActionResult<Doctor>> UpdateAsync(Guid doctorId, Guid patientId, [FromBody] PatientREST patientREST)
        {
            if (patientREST.Age <= 0)
            {
                return BadRequest("Age cannot be lower than 0");
            }
            var patient = await _service.UpdateAsync(doctorId, patientId, patientREST);

            if (patient == null)
            {
                return NotFound("Patient with this ID not found");
            }

            return Ok(patient);
        }

        [HttpGet("get-patients")]
        public async Task<ActionResult<Doctor>> GetAllPatientsAsync(Guid doctorId)
        {
            return await _service.GetAllPatientsAsync(doctorId);
        }

        [HttpGet("get-paginated-patients")]
        public async Task<ActionResult<Doctor>> GetDoctorWithPatientsPaginatedAsync(Guid doctorId, int page, int pageSize )
        {
            return await _service.GetDoctorWithPatientsPaginatedAsync(doctorId, page, pageSize);
        }

        [HttpDelete("delete/{patientId}")]
        public async Task<ActionResult> DeleteAsync(Guid patientId)
        {
            if (await _service.DeleteAsync(patientId))
            {
                return Ok("Patient deleted");
            }

            return NotFound("Patient with this ID not found");
        }
    }
}
