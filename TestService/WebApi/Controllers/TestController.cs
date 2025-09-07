using Application.Dtos;
using Application.Dtos.AnswerDtos;
using Application.Dtos.QuestionDtos;
using Application.Dtos.TestDtos;
using Application.Services.Interfaces;
using AutoMapper;
using Azure.Core;
using Infrastructure.Repositories.Intefaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseApiUrl;
        private readonly ITestService _testService;
        private readonly ITestQuestionService _testQuestionService;

        private readonly IMapper _mapper;
        public TestController(HttpClient httpClient, ITestService testService, IMapper mapper, ITestQuestionService testQuestionService, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _testService = testService;
            _mapper = mapper;
            _testQuestionService = testQuestionService;
            _baseApiUrl = configuration["ApiSettings:BaseApiUrl"] ?? throw new InvalidOperationException("_baseApiUrl is not configured.");
        }

        [HttpPost("create-a-test")]
        [Authorize]
        public async Task<IActionResult> CreateTestAsync()
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("UserId not found in token");

            Guid userId = Guid.Parse(userIdClaim);

            var listTests = await _testService.GetAllTestByUserIdAsync(userId);
            var latestTest = listTests.Items.FirstOrDefault();

            if ( latestTest != null && DateTime.TryParseExact(latestTest.CreatedDate, "dd/MM/yyyy HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var createdDate))
            {
                if (latestTest.Score < 0 && createdDate.AddMinutes(latestTest.Duration) > DateTime.Now)
                    return Ok(latestTest.Id);
            }

            var response = await _httpClient.GetAsync($"{_baseApiUrl}/Question/generate/all");
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Request Failed");

            var json = await response.Content.ReadAsStringAsync();
            var questionIds = JsonSerializer.Deserialize<IEnumerable<Guid>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (questionIds == null)
                return NotFound("No question found");
            var testId = await _testService.CreateTestAsync(userId, questionIds);
            return Ok(testId);
        }
        [HttpPost("submit/{testId}")]
        [Authorize]
        public async Task<IActionResult> SubmitTestAsync(Guid testId, [FromBody] SubmitTestRequestDto submitTestRequestDto)
        {
            await _testService.SubmitAnswerInATest(Guid.NewGuid(), testId, submitTestRequestDto.listAnswerIds, submitTestRequestDto.listTrueAnswerIds);
            return Ok(new { Message = "Submit successfully" });
        }
        [HttpGet("user-test/all/{page}/{pageSize}")]
        [Authorize]
        public async Task<IActionResult> GetAllTestAsync(int page = 1, int pageSize = 10)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("UserId not found in token");

            Guid userId = Guid.Parse(userIdClaim);

            if (userId == Guid.Empty)
            {
                return BadRequest("invalid id");
            }

            var result = await _testService.GetAllTestByUserIdAsync(userId, page, pageSize);

            return Ok(result);
        }
        [HttpGet("test-detail/{testId}")]
        [Authorize]
        public async Task<IActionResult> GetTestDetailAsync(Guid testId)
        {
            if (testId == Guid.Empty)
                return NotFound();

            var accessToken = HttpContext.Request.Cookies["AccessToken"];

            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return Unauthorized("Missing access token in cookie");
            }

            if (string.IsNullOrWhiteSpace(accessToken))
                return Unauthorized("Missing access token");

            var questionIds = await _testQuestionService.GetQuestionIdsInTestIdAsync(testId);

            var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseApiUrl}/Question/get-by-list/detail")
            {
                Content = new StringContent(JsonSerializer.Serialize(questionIds), Encoding.UTF8, "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Replace("Bearer ", ""));

            var detailResponse = await _httpClient.SendAsync(request);
            if (!detailResponse.IsSuccessStatusCode)
                return StatusCode((int)detailResponse.StatusCode, "Request Failed when fetching question details");

            var summaryJson = await detailResponse.Content.ReadAsStringAsync();
            var listQuestionResponse = JsonSerializer.Deserialize<IEnumerable<QuestionDetailDto>>(summaryJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? Enumerable.Empty<QuestionDetailDto>();

            var result = await _testService.GetTestDetailAsync(testId, listQuestionResponse);
            return Ok(result);
        }

    }
}
