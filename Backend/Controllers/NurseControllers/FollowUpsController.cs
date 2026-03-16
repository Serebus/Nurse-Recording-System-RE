using Microsoft.AspNetCore.Mvc;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.IFollowUps;
using NurseRecordingSystem.Model.DTO.NurseServicesDTOs.FollowUpDTOs;

namespace NurseRecordingSystem.Controllers.NurseControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowUpsController : ControllerBase
    {
        private readonly ICreateFollowUp _createFollowUp;
        private readonly IUpdateFollowUp _updateFollowUp;
        private readonly IDeleteFollowUp _deleteFollowUp;

        public FollowUpsController(
            ICreateFollowUp createFollowUp,
            IUpdateFollowUp updateFollowUp,
            IDeleteFollowUp deleteFollowUp)
        {
            _createFollowUp = createFollowUp;
            _updateFollowUp = updateFollowUp;
            _deleteFollowUp = deleteFollowUp;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFollowUp([FromBody] CreateFollowUpDTO dto)
        {
            try {
                var newId = await _createFollowUp.CreateFollowUpAsync(dto);
                return Ok(new { FollowUpId = newId, Message = "Follow-up created successfully." });
            } catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFollowUp(int id, [FromBody] UpdateFollowUpDTO dto)
        {
            try {
                await _updateFollowUp.UpdateFollowUpAsync(id, dto);
                return Ok(new { Message = "Follow-up updated successfully." });
            } 
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFollowUp(int id, [FromQuery] string deletedBy)
        {
            try {
                if (string.IsNullOrWhiteSpace(deletedBy)) return BadRequest("deletedBy is required");
                
                await _deleteFollowUp.DeleteFollowUpAsync(id, deletedBy);
                return Ok(new { Message = "Follow-up deleted successfully." });
            }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }
    }
}