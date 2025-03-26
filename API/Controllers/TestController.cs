using API.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public ActionResult<ApiResponse<object>> TestDeployment()
    {
        var deploymentInfo = new
        {
            Timestamp = DateTime.UtcNow,
            Version = "1.0.0",
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown"
        };

        return Ok(new ApiResponse<object>
        {
            Data = deploymentInfo,
            Message = "Test endpoint working",
            Success = true
        });
    }
}