﻿using Microsoft.AspNetCore.Mvc;
using NurseRecordingSystem.Contracts.ServiceContracts.IHelperServices.IHelperClinicStatus;

[Route("api/helper/clinicstatus")]
[ApiController]
public class HelperClinicStatusController : ControllerBase
{
    private readonly IViewClinicStatus _viewService;

    public HelperClinicStatusController(IViewClinicStatus viewService)
    {
        _viewService = viewService;
    }

    [HttpGet("view_all_clinic_status")]
    public async Task<IActionResult> ViewAllStatus()
    {
        try
        {
            var statusList = await _viewService.ViewAllAsync();

            if (statusList == null || !statusList.Any())
            {
                return Ok(new { message = "No clinic status records found." });
            }

            return Ok(statusList);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
        }
    }
}