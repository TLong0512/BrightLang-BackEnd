using Application.Abstraction.Services;
using Application.Dtos.QuestionBankService;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Infrastructure.Services;

public class QuestionbankService : IQuestionbankService
{
    private readonly HttpClient httpClient;

    public QuestionbankService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<List<LevelViewDto>> GetAllLevelsAsync()
    {
        List<LevelViewDto>? result = await httpClient.GetFromJsonAsync<List<LevelViewDto>>("/api/Level");
        return result ?? new List<LevelViewDto>();
    }

    public async Task<LevelViewDto?> GetLevelByIdAsync(Guid id)
    {
        HttpResponseMessage response = await httpClient.GetAsync($"/api/Level/{id}");
        if (response.IsSuccessStatusCode == false)
            return null;

        return await response.Content.ReadFromJsonAsync<LevelViewDto>();
    }



    public async Task<List<SkillLevelViewDto>> GetAllSkillLevelsAsync()
    {
        List<SkillLevelViewDto>? result = await httpClient.GetFromJsonAsync<List<SkillLevelViewDto>>("/api/SkillLevel");
        return result ?? new List<SkillLevelViewDto>();
    }

    public async Task<SkillLevelViewDto?> GetSkillLevelByIdAsync(Guid id)
    {
        HttpResponseMessage response = await httpClient.GetAsync($"/api/SkillLevel/{id}");
        if (response.IsSuccessStatusCode == false)
            return null;
        
        return await response.Content.ReadFromJsonAsync<SkillLevelViewDto>();
    }




    public async Task<List<RangeViewDto>> GetAllRangesAsync()
    {
        List<RangeViewDto>? result = await httpClient.GetFromJsonAsync<List<RangeViewDto>>("/api/Range");
        return result ?? new List<RangeViewDto>();
    }

    public async Task<List<RangeViewDto>?> GetAllRangesBySkillLevelIdAsync(Guid skillLevelId)
    {
        HttpResponseMessage response = await httpClient.GetAsync($"/api/Range/filter/skill-level/{skillLevelId}");
        if (response.IsSuccessStatusCode == false)
            return null;

        return await response.Content.ReadFromJsonAsync<List<RangeViewDto>>();
    }

    public async Task<RangeViewDto?> GetRangeByIdAsync(Guid id)
    {
        HttpResponseMessage response = await httpClient.GetAsync($"/api/Range/{id}");
        if (response.IsSuccessStatusCode == false)
            return null;

        return await response.Content.ReadFromJsonAsync<RangeViewDto>();
    }

    
}
