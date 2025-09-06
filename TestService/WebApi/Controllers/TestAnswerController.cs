using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestAnswerController : ControllerBase
    {
        private readonly ITestAnswerSevice _testAnswerService;
        public TestAnswerController(ITestAnswerSevice testAnswerSevice)
        {
            _testAnswerService = testAnswerSevice;
        }
        [HttpPost("{testId}")]
        [Authorize]
        public async Task<IActionResult> SaveAnswerInTest(Guid testId, [FromBody] IEnumerable<Guid> answers)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("UserId not found in token");

            Guid userId = Guid.Parse(userIdClaim);

            await _testAnswerService.AddTestAnswerAsync(testId, answers, userId);

            return Ok(new {Message = "Added answer for test successfully" });
        }
    }
}
