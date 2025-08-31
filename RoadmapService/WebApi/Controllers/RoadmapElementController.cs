using Application.Abstraction;
using Application.Abstraction.Services;
using Application.Dtos.QuestionBankService;
using Application.Dtos.RoadmapElementDtos;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoadmapElementController : ControllerBase
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IQuestionbankService questionbankService;

    public RoadmapElementController(IUnitOfWork unitOfWork, IQuestionbankService questionbankService)
    {
        this.unitOfWork = unitOfWork;
        this.questionbankService = questionbankService;
    }

    [HttpGet("roadmap/{roadmapId:guid}")]
    public async Task<ActionResult<List<RoadmapElementDto>>> GetAllRoadmapElementsByRoadmapId(Guid roadmapId)
    {
        Roadmap? existedRoadmap = await unitOfWork.Roadmaps.GetByIdAsync(roadmapId);
        if (existedRoadmap == null) return NotFound();

        IEnumerable<RoadmapElement> roadmapElements = await unitOfWork.RoadmapElements
            .FindAsync(e => e.RoadmapId == roadmapId);


        // TODO: caching the range
        RoadmapElementDto[] roadmapElementsDto = await Task.WhenAll(
            roadmapElements.Select(async r => new RoadmapElementDto
            {
                Id = r.Id,
                RoadmapId = roadmapId,
                Order = r.Order,
                QuestionPerDay = r.QuestionPerDay,
                Range = await questionbankService.GetRangeByIdAsync(r.RangeId)
                        ?? new RangeViewDto { Name = " Unknown range." }
            })
        );

        return Ok(roadmapElementsDto.ToList().OrderBy(r => r.Order));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoadmapElementDto>> GetRoadmapElementById(Guid id)
    {
        RoadmapElement? adsf = await unitOfWork.RoadmapElements.GetByIdAsync(id);
        if (adsf == null) return NotFound();
        return Ok(new RoadmapElementDto
        {
            Id = adsf.Id,
            Order = adsf.Order,
            QuestionPerDay = adsf.QuestionPerDay,
            RoadmapId = adsf.RoadmapId,
            Range = await questionbankService.GetRangeByIdAsync(adsf.RangeId)
                    ?? new RangeViewDto { Name = " Unknown range." }
        });
    }

    // roadmap elements are immutable. even admin can't change it.

    // TODO: consider caching result and update on question bank's change.
}
