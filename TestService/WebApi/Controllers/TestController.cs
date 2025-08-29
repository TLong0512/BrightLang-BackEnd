using Application.Dtos;
using Application.Dtos.AnswerDtos;
using Application.Dtos.QuestionDtos;
using Application.Dtos.TestDtos;
using Application.Services.Interfaces;
using AutoMapper;
using Azure.Core;
using Infrastructure.Repositories.Intefaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private const string BaseApiUrl = "http://localhost:5003/api";
        private readonly ITestService _testService;
        private readonly ITestQuestionService _testQuestionService;

        private readonly IMapper _mapper;
        public TestController(HttpClient httpClient, ITestService testService, IMapper mapper, ITestQuestionService testQuestionService)
        {
            _httpClient = httpClient;
            _testService = testService;
            _mapper = mapper;
            _testQuestionService = testQuestionService;
        }

        [HttpPost("create-a-test/{numberPerSkillLevel?}")]
        public async Task<IActionResult> CreateTestAsync(int numberPerSkillLevel = 2)
        {
            var response = await _httpClient.GetAsync($"{BaseApiUrl}/Question/generate/all/{numberPerSkillLevel}");
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Request Failed");
            var json = await response.Content.ReadAsStringAsync();
            var questionIds = JsonSerializer.Deserialize<IEnumerable<Guid>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var testId = await _testService.CreateTestAsync(new Guid(), questionIds);

            var requestContent = new StringContent(
                    JsonSerializer.Serialize(questionIds),
                    Encoding.UTF8,
                    "application/json");
            var summaryResponse = await _httpClient.PostAsync($"{BaseApiUrl}/Question/get-by-list/summary", requestContent);
            if (!summaryResponse.IsSuccessStatusCode)
                return StatusCode((int)summaryResponse.StatusCode, "Request Failed when fetching question summaries");
            var summaryJson = await summaryResponse.Content.ReadAsStringAsync();
            var listQuestionResponse = JsonSerializer.Deserialize<IEnumerable<QuestionSummaryDto>>(summaryJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var testInprogressDto = new TestInProgressDto
            {
                TestId = testId,
                ListQuestion = listQuestionResponse
            };
            return Ok(testInprogressDto);
        }
        [HttpPost("submit/{testId}")]
        public async Task<IActionResult> SubmitTestAsync(Guid testId, [FromBody] SubmitTestRequestDto submitTestRequestDto)
        {
            await _testService.SubmitAnswerInATest(Guid.NewGuid(), testId, submitTestRequestDto.listAnswerIds, submitTestRequestDto.listTrueAnswerIds);
            return Ok(("Result has been saved"));
        }
        [HttpGet("all-test/{userId}")]
        public async Task<IActionResult> GetAllTestAsync(Guid userId, int page = 1, int pageSize = 10)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest("invalid id");
            }

            var result = await _testService.GetAllTestByUserIdAsync(userId, page, pageSize);

            if (result == null || !result.Items.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }
        [HttpGet("test-detail/{testId}")]
        public async Task<IActionResult> GetTestDetailAsync(Guid testId)
        {
            if (testId == Guid.Empty)
            {
                return NotFound();
            }
            var questionIds = await _testQuestionService.GetQuestionIdsInTestIdAsync(testId);
            var requestContent = new StringContent(
                                JsonSerializer.Serialize(questionIds),
                                Encoding.UTF8,
                                "application/json");
            var detailResponse = await _httpClient.PostAsync($"{BaseApiUrl}/Question/get-by-list/detail", requestContent);
            if (!detailResponse.IsSuccessStatusCode)
                return StatusCode((int)detailResponse.StatusCode, "Request Failed when fetching question details");
            var summaryJson = await detailResponse.Content.ReadAsStringAsync();
            var listQuestionResponse = JsonSerializer.Deserialize<IEnumerable<QuestionDetailDto>>(summaryJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            var result = await _testService.GetTestDetailAsync(testId,listQuestionResponse, questionIds);
            return Ok(result);
        }
    }
}
