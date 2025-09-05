using Application.Abstraction;
using Application.Abstraction.Services;
using Application.Dtos.QuestionBankService;
using Application.Dtos.RoadmapDtos;
using Application.Dtos.RoadmapElementDtos;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

// everyone can see what available roadmap is.
// but no one can change the roadmap.
[Route("api/[controller]")]
[ApiController]
public class RoadmapsController : ControllerBase
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IQuestionbankService questionbankService;

    public RoadmapsController(IUnitOfWork unitOfWork, IQuestionbankService questionbankService)
    {
        this.unitOfWork = unitOfWork;
        this.questionbankService = questionbankService;
    }



    // used when checking all available roadmap
    [HttpGet]
    public async Task<ActionResult<List<RoadmapGeneralDto>>> GetAllRoadmaps()
    {
        IEnumerable<Roadmap> roadmaps = await unitOfWork.Roadmaps.GetAllAsync();

        List<RoadmapGeneralDto> roadmapGeneralDtos = new List<RoadmapGeneralDto>();
        foreach (var roadmap in roadmaps)
        {
            IEnumerable<RoadmapElement> roadmapElements = await unitOfWork.RoadmapElements
                .FindAsync(r => r.RoadmapId == roadmap.Id);

            double average = roadmapElements.Any()
                ? roadmapElements.Select(r => r.QuestionPerDay).Average()
                : 0;

            IEnumerable<UserRoadmap> userRoadmaps = await unitOfWork.UserRoadmaps
                .FindAsync(r => r.RoadmapId == roadmap.Id);

            LevelViewDto levelStart = await questionbankService.GetLevelByIdAsync(roadmap.LevelStartId)
                                ?? new LevelViewDto { Name = "Unknown start level." };

            LevelViewDto levelEnd = await questionbankService.GetLevelByIdAsync(roadmap.LevelEndId)
                                ?? new LevelViewDto { Name = "Unknown end level." };

            var roadmapGeneralDto = new RoadmapGeneralDto
            {
                Id = roadmap.Id,
                Name = roadmap.Name,
                QuestionPerDay = average,
                SignupCount = userRoadmaps.Count(),
                TimeRequired = roadmapElements.Count(),
                LevelStart = levelStart,
                LevelEnd = levelEnd,
            };
            roadmapGeneralDtos.Add(roadmapGeneralDto);
        }
        return Ok(roadmapGeneralDtos);
    }

    // used when checking a specific roadmap
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RoadmapDetailDto>> GetRoadmap(Guid id)
    {
        Roadmap? roadmap = await unitOfWork.Roadmaps.GetByIdAsync(id);
        if (roadmap == null) return NotFound();

        IEnumerable<RoadmapElement> roadmapElements = await unitOfWork.RoadmapElements
            .FindAsync(e => e.RoadmapId == roadmap.Id);

        double average = roadmapElements.Any()
                ? roadmapElements.Select(r => r.QuestionPerDay).Average()
                : 0;

        IEnumerable<UserRoadmap> userRoadmaps = await unitOfWork.UserRoadmaps
                .FindAsync(r => r.RoadmapId == roadmap.Id);

        LevelViewDto levelStart = await questionbankService.GetLevelByIdAsync(roadmap.LevelStartId)
                            ?? new LevelViewDto { Name = "Unknown start level." };

        LevelViewDto levelEnd = await questionbankService.GetLevelByIdAsync(roadmap.LevelEndId)
                            ?? new LevelViewDto { Name = "Unknown end level." };

        // TODO: maybe questionbankService should give me a GetRangesBymultipleId
        // performance issue: N + 1 http request
        RoadmapElementDto[] roadmapElementDtos = await Task.WhenAll(
            roadmapElements.Select(async e => new RoadmapElementDto
            {
                Id = e.Id,
                Order = e.Order,
                RoadmapId = roadmap.Id,
                QuestionPerDay = e.QuestionPerDay,
                Range = await questionbankService.GetRangeByIdAsync(e.RangeId)
                        ?? new RangeViewDto { Name = "Unknown range." }
            })
        );

        return Ok(new RoadmapDetailDto
        {
            Id = roadmap.Id,
            Name = roadmap.Name,
            QuestionPerDay = average,
            SignupCount = userRoadmaps.Count(),
            TimeRequired = roadmapElements.Count(),
            LevelStart = levelStart,
            LevelEnd = levelEnd,
            RoadmapElements = roadmapElementDtos.ToList()
        });
    }
    // TODO: consider caching result and update on question bank's change.
}
