using Application.Dtos.ExamTypeDto;
using Application.Dtos.LevelDto;
using Application.Dtos.QuestionDto;
using Application.Dtos.RageDto;
using Application.Dtos.SkillLevelDto;
using Application.Services.Interface;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Implementation
{
    public class QuestionService : IQuestionService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseApiUrl;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public QuestionService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _baseApiUrl = configuration["ApiSettings:QuestionApiUrl"] ?? throw new InvalidOperationException("_baseApiUrl is not configured.");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<ExamTypeViewDto>> GetAllExamTypesAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseApiUrl}/ExamType");
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<IEnumerable<ExamTypeViewDto>>();
            return data ?? new List<ExamTypeViewDto>();
        }

        public async Task<IEnumerable<LevelViewDto>> GetLevelsByExamTypeIdAsync(Guid examTypeId)
        {
            var response = await _httpClient.GetAsync($"{_baseApiUrl}/level/filter/exam-type/{examTypeId}");
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<IEnumerable<LevelViewDto>>();
            return data ?? new List<LevelViewDto>();
        }

        public async Task<IEnumerable<SkillLevelViewDto>> GetSkillLevelByLevelIdAsync(Guid levelId)
        {
            var response = await _httpClient.GetAsync($"{_baseApiUrl}/skillLevel/filter/level/{levelId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<SkillLevelViewDto>>() ?? new List<SkillLevelViewDto>();
        }

        public async Task<IEnumerable<RangeViewDto>> GetRangesBySkillLevelIdAsync(Guid skillLevelId)
        {
            var response = await _httpClient.GetAsync($"{_baseApiUrl}/range/filter/skill-level/{skillLevelId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<RangeViewDto>>() ?? new List<RangeViewDto>();
        }

        public async Task<IEnumerable<QuestionSummaryDto>> GenerateQuestionsByRangeIdAsync(Guid rangeId)
        {
            if (rangeId == Guid.Empty)
                return Enumerable.Empty<QuestionSummaryDto>();

            // Lấy token từ cookie
            var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new UnauthorizedAccessException("Missing access token in cookie");

            // 1. Gọi API lấy list questionId
            var requestIds = new HttpRequestMessage(HttpMethod.Get, $"{_baseApiUrl}/question/generate/range/{rangeId}");
            requestIds.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Replace("Bearer ", ""));

            var idsResponse = await _httpClient.SendAsync(requestIds);
            if (!idsResponse.IsSuccessStatusCode)
                throw new Exception($"Failed to fetch question ids. Status: {idsResponse.StatusCode}");

            var questionIds = await idsResponse.Content.ReadFromJsonAsync<IEnumerable<Guid>>() ?? new List<Guid>();
            if (!questionIds.Any())
                return Enumerable.Empty<QuestionSummaryDto>();

            // 2. Gọi API lấy summary theo list Id
            var requestSummary = new HttpRequestMessage(HttpMethod.Post, $"{_baseApiUrl}/question/get-by-list/summary")
            {
                Content = new StringContent(JsonSerializer.Serialize(questionIds), Encoding.UTF8, "application/json")
            };
            requestSummary.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Replace("Bearer ", ""));

            var summaryResponse = await _httpClient.SendAsync(requestSummary);
            if (!summaryResponse.IsSuccessStatusCode)
                throw new Exception($"Failed to fetch question summaries. Status: {summaryResponse.StatusCode}");

            var summaries = await summaryResponse.Content.ReadFromJsonAsync<IEnumerable<QuestionSummaryDto>>()
                           ?? new List<QuestionSummaryDto>();

            return summaries;
        }

        public async Task<IEnumerable<QuestionDetailDto>> ShowDetailQuestionsByRangeIdAsync(Guid rangeId)
        {
            if (rangeId == Guid.Empty)
                return Enumerable.Empty<QuestionDetailDto>();

            // Lấy token từ cookie
            var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new UnauthorizedAccessException("Missing access token in cookie");

            // 1. Gọi API lấy list Id
            var requestIds = new HttpRequestMessage(HttpMethod.Get, $"{_baseApiUrl}/question/generate/range/{rangeId}");
            requestIds.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Replace("Bearer ", ""));

            var idsResponse = await _httpClient.SendAsync(requestIds);
            if (!idsResponse.IsSuccessStatusCode)
                throw new Exception($"Failed to fetch question ids. Status: {idsResponse.StatusCode}");

            var questionIds = await idsResponse.Content.ReadFromJsonAsync<IEnumerable<Guid>>() ?? new List<Guid>();
            if (!questionIds.Any())
                return Enumerable.Empty<QuestionDetailDto>();

            // 2. Gọi API lấy detail theo list Id
            var requestDetail = new HttpRequestMessage(HttpMethod.Post, $"{_baseApiUrl}/question/get-by-list/detail")
            {
                Content = new StringContent(JsonSerializer.Serialize(questionIds), Encoding.UTF8, "application/json")
            };
            requestDetail.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Replace("Bearer ", ""));

            var detailResponse = await _httpClient.SendAsync(requestDetail);
            if (!detailResponse.IsSuccessStatusCode)
                throw new Exception($"Failed to fetch question details. Status: {detailResponse.StatusCode}");

            var details = await detailResponse.Content.ReadFromJsonAsync<IEnumerable<QuestionDetailDto>>()
                           ?? new List<QuestionDetailDto>();

            return details;
        }
    }
}
