using Application.Dtos;
using Application.Dtos.QuestionDtos;
using Application.Dtos.TestDtos;
using Application.Services.Interfaces;
using AutoMapper;
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
        private readonly IMapper _mapper;
        public TestController(HttpClient httpClient, ITestService testService, IMapper mapper)
        {
            _httpClient = httpClient;
            _testService = testService;
            _mapper = mapper;
        }

        [HttpPost("create-a-test/{numberPerSkillLevel?}")]
        public async Task<IActionResult> DoATest(int numberPerSkillLevel = 2)
        {
            var response = await _httpClient.GetAsync($"{BaseApiUrl}/Question/generate/all/{numberPerSkillLevel}");
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Request Failed");
            var json = await response.Content.ReadAsStringAsync();
            var questionIds = JsonSerializer.Deserialize<List<Guid>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var testId = await _testService.CreateTestAsync(new Guid(), questionIds);

            var requestContent = new StringContent(
                    JsonSerializer.Serialize(questionIds),
                    Encoding.UTF8,
                    "application/json");
            var summaryResponse = await _httpClient.PostAsync($"{BaseApiUrl}/Question/get-by-list/summary", requestContent);
            if (!summaryResponse.IsSuccessStatusCode)
                return StatusCode((int)summaryResponse.StatusCode, "Request Failed when fetching question summaries");
            var summaryJson = await summaryResponse.Content.ReadAsStringAsync();
            var listQuestionResponse = JsonSerializer.Deserialize<List<QuestionSummaryDto>>(summaryJson, new JsonSerializerOptions
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
        public async Task<IActionResult> SubmitTest(Guid testId, [FromBody] List<Guid> listAnswerIds)
        {
            await _testService.SubmitAnswerInATest(Guid.NewGuid(), testId, listAnswerIds);
            return Ok(("Result has been saved"));
        }
    }
}
