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
        public ActionResult<Patient> Create(Guid doctorId, [FromBody] PatientREST patientREST)
        {
            if (patientREST.Age <= 0)
            {
                return BadRequest("Age cannot be lower than 0");
            }
            var patient = _service.Create(doctorId, patientREST);
            return Ok(patient);
        }

        [HttpPut("update/{patientId}")]
        public ActionResult<Doctor> Update(Guid doctorId, Guid patientId, [FromBody] PatientREST patientREST)
        {
            if (patientREST.Age <= 0)
            {
                return BadRequest("Age cannot be lower than 0");
            }
            var patient = _service.Update(doctorId, patientId, patientREST);

            if (patient == null)
            {
                return NotFound("Patient with this ID not found");
            }

            return Ok(patient);
        }

        [HttpGet("get-patients")]
        public ActionResult<Doctor> GetAllPatients(Guid doctorId)
        {
            return _service.GetAllPatients(doctorId);
        }

        [HttpDelete("delete/{patientId}")]
        public ActionResult Delete(Guid patientId)
        {
            if (_service.Delete(patientId))
            {
                return Ok("Patient deleted");
            }

            return NotFound("Patient with this ID not found");
        }
    }
}
