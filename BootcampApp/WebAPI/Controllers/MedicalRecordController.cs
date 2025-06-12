using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using Model.Models;
using Service.Interface;
using Service.ServiceImpl;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("patient/{patientId}/record")]
    public class MedicalRecordController : Controller
    {
        private readonly IMedicalRecordService _service;

        public MedicalRecordController(IMedicalRecordService service)
        {
            this._service = service;
        }

        [HttpPost("create")]
        public async Task<ActionResult<MedicalRecord>> CreateAsync(Guid patientId, [FromBody] MedicalRecordREST medicalRecordREST)
        {
            var record = await _service.CreateAsync(patientId, medicalRecordREST);
            return Ok(record);
        }

        [HttpPut("update/{recordId}")]
        public async Task<ActionResult<MedicalRecord>> UpdateAsync(Guid patientId, Guid recordId,  [FromBody]MedicalRecordREST medicalRecordREST)
        {
            var record = await _service.UpdateAsync(patientId, recordId, medicalRecordREST);

            if (record == null)
            {
                return NotFound("Record with this ID not found");
            }

            return Ok(record);
        }

        [HttpGet("with-record")]
        public async Task<IActionResult> GetPatientWithRecordAsync(Guid patientId)
        {
            var result = await _service.GetPatientWithRecordAsync(patientId);
            if (result == null)
            {
                return NotFound("Patient with this ID not found");
            }
            return Ok(result);
        }

    }
}
