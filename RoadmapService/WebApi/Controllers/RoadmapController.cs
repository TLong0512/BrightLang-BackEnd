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
public class RoadmapController : ControllerBase
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IQuestionbankService questionbankService;

    public RoadmapController(IUnitOfWork unitOfWork, IQuestionbankService questionbankService)
    {
        this.unitOfWork = unitOfWork;
        this.questionbankService = questionbankService;
    }



    // used when checking all available roadmap
    [HttpGet]
    public async Task<ActionResult<List<RoadmapGeneralDto>>> GetAllRoadmaps()
    {
        IEnumerable<Roadmap> roadmaps = await unitOfWork.Roadmaps.GetAllAsync();
        RoadmapGeneralDto[] roadmapDtos = await Task.WhenAll(
            roadmaps.Select(async roadmap =>
            {
                LevelViewDto levelStart = await questionbankService.GetLevelByIdAsync(roadmap.LevelStartId)
                                    ?? new LevelViewDto { Name = "Unknown start level." };
                LevelViewDto levelEnd = await questionbankService.GetLevelByIdAsync(roadmap.LevelEndId)
                                    ?? new LevelViewDto { Name = "Unknown end level." };
                return new RoadmapGeneralDto
                {
                    Id = roadmap.Id,
                    Name = roadmap.Name,
                    LevelStart = levelStart,
                    LevelEnd = levelEnd,
                };
            })
        );
        return Ok(roadmapDtos.ToList());
    }

    // used when checking a specific roadmap
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RoadmapSpecificDto>> GetRoadmap(Guid id)
    {
        Roadmap? roadmap = await unitOfWork.Roadmaps.GetByIdAsync(id);
        if (roadmap == null) return NotFound();

        IEnumerable<RoadmapElement> roadmapElements = await unitOfWork.RoadmapElements
            .FindAsync(e => e.RoadmapId == roadmap.Id);

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

        return Ok(new RoadmapSpecificDto
        {
            Id = roadmap.Id,
            Name = roadmap.Name,
            LevelStart = await questionbankService.GetLevelByIdAsync(roadmap.LevelStartId)
                            ?? new LevelViewDto { Name = "Unknown start level." },
            LevelEnd = await questionbankService.GetLevelByIdAsync(roadmap.LevelEndId)
                            ?? new LevelViewDto { Name = "Unknown end level." },
            RoadmapElements = roadmapElementDtos.ToList()
        });
    }

    // roadmaps are immutable. even admin can't change it.

    // TODO: consider caching result and update on question bank's change.
}
