using Application.Abstraction;
using Application.Abstraction.Services;
using Application.Dtos.ProcessDtos;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "User")]
public class ProcessController : ControllerBase
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IQuestionbankService questionbankService;
    private readonly ICurrentUserService currentUserService;

    public ProcessController(IUnitOfWork unitOfWork, IQuestionbankService questionbankService, ICurrentUserService currentUserService)
    {
        this.unitOfWork = unitOfWork;
        this.questionbankService = questionbankService;
        this.currentUserService = currentUserService;
    }

    [HttpGet("/userRoadmapId/{userRoadmapId}/roadmapElementId/{roadmapElementId}")]
    public async Task<ActionResult<ProcessDto>> GetProcessById(Guid userRoadmapId, Guid roadmapElementId)
    {
        Process? process = await unitOfWork.Processes.GetByIdAsync(userRoadmapId, roadmapElementId);
        if (process == null) return NotFound();

        // verify ownership to see, also checking for valid userRoadmapId
        UserRoadmap? userRoadmap = await unitOfWork.UserRoadmaps.GetByIdAsync(process.UserRoadmapId);
        if (userRoadmap == null ||
            currentUserService.UserId == null ||
            userRoadmap.UserId != currentUserService.UserId) return NotFound();

        return Ok(new ProcessDto
        {
            UserRoadmapId = process.UserRoadmapId,
            RoadmapElementId = process.RoadmapElementId,
            StartDate = process.StartDate,
            EndDate = process.EndDate,
            IsFinished = process.IsFinished,
            IsOpened = process.IsOpened,
        });
    }

    [HttpGet("/userRoadmapId/{userRoadmapId}")]
    public async Task<ActionResult<List<ProcessDto>>> GetProcessByUserRoadmapId(Guid userRoadmapId)
    {
        // verify ownership to see, also checking for valid userRoadmapId
        UserRoadmap? userRoadmap = await unitOfWork.UserRoadmaps.GetByIdAsync(userRoadmapId);
        if (userRoadmap == null ||
            currentUserService.UserId == null ||
            userRoadmap.UserId != currentUserService.UserId) return NotFound();

        IEnumerable<Process> processes = await unitOfWork.Processes.FindAsync(r => r.UserRoadmapId == userRoadmapId);
        IEnumerable<ProcessDto> processDtos = processes.Select(p => new ProcessDto
        {
            UserRoadmapId = p.UserRoadmapId,
            RoadmapElementId = p.RoadmapElementId,
            StartDate = p.StartDate,
            EndDate = p.EndDate,
            IsFinished = p.IsFinished,
            IsOpened = p.IsOpened,
        });

        return Ok(processDtos.ToList());
    }

    [HttpPost]
    public async Task<ActionResult<ProcessDto>> AddRoadmap([FromBody] ProcessPostDto processPostDto)
    {
        UserRoadmap? userRoadmap = await unitOfWork.UserRoadmaps.GetByIdAsync(processPostDto.UserRoadmapId);
        if (userRoadmap == null)
            return BadRequest("User roadmap not found.");

        Roadmap? roadmap = await unitOfWork.Roadmaps.GetByIdAsync(userRoadmap.RoadmapId);
        if (roadmap == null)
            return BadRequest("Original roadmap not found."); // BE fault.

        RoadmapElement? roadmapElement = await unitOfWork.RoadmapElements.GetByIdAsync(processPostDto.RoadmapElementId);
        if (roadmapElement == null)
            return BadRequest("Roadmap element not found.");

        if (roadmapElement.RoadmapId != roadmap.Id)
            return BadRequest("Roadmap element doesn't belong to the original roadmap.");

        if (await unitOfWork.Processes.GetByIdAsync(processPostDto.UserRoadmapId, processPostDto.RoadmapElementId) != null)
            return BadRequest("Process already exist.");

        Process newProcess = new Process
        {
            RoadmapElementId = processPostDto.RoadmapElementId,
            UserRoadmapId = processPostDto.UserRoadmapId,
            StartDate = processPostDto.StartDate,
            EndDate = processPostDto.EndDate,
            IsFinished = processPostDto.IsFinished,
            IsOpened = processPostDto.IsOpened,
        };
        await unitOfWork.Processes.AddAsync(newProcess);
        await unitOfWork.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetProcessById), 
            new { userRoadmapId = newProcess.UserRoadmapId, roadmapElementId = newProcess.RoadmapElementId },
            new ProcessDto
            {
                RoadmapElementId = newProcess.RoadmapElementId,
                UserRoadmapId = newProcess.UserRoadmapId,
                StartDate = newProcess.StartDate,
                EndDate = newProcess.EndDate,
                IsFinished = newProcess.IsFinished,
                IsOpened = newProcess.IsOpened,
            }
        );
    }


    // but for managing process, toggle fields are allowed
    [HttpPut("/userRoadmapId/{userRoadmapId}/roadmapElementId/{roadmapElementId}/toggle-finish")]
    public async Task<ActionResult> ToggleProcessIsFinished(Guid userRoadmapId, Guid roadmapElementId)
    {
        Process? process = await unitOfWork.Processes.GetByIdAsync(userRoadmapId, roadmapElementId);
        if (process == null) return NotFound();

        // verify ownership to see, also checking for valid userRoadmapId
        UserRoadmap? userRoadmap = await unitOfWork.UserRoadmaps.GetByIdAsync(process.UserRoadmapId);
        if (userRoadmap == null ||
            currentUserService.UserId == null ||
            userRoadmap.UserId != currentUserService.UserId) return NotFound();

        // toggle
        process.IsFinished = !process.IsFinished;
        await unitOfWork.SaveChangesAsync();

        return Ok(process);
    }

    [HttpPut("/userRoadmapId/{userRoadmapId}/roadmapElementId/{roadmapElementId}/toggle-open")]
    public async Task<ActionResult> ToggleProcessIsOpened(Guid userRoadmapId, Guid roadmapElementId)
    {
        Process? process = await unitOfWork.Processes.GetByIdAsync(userRoadmapId, roadmapElementId);
        if (process == null) return NotFound();

        // verify ownership to see, also checking for valid userRoadmapId
        UserRoadmap? userRoadmap = await unitOfWork.UserRoadmaps.GetByIdAsync(process.UserRoadmapId);
        if (userRoadmap == null ||
            currentUserService.UserId == null ||
            userRoadmap.UserId != currentUserService.UserId) return NotFound();

        // toggle
        process.IsOpened = !process.IsOpened;
        await unitOfWork.SaveChangesAsync();

        return Ok(process);
    }


    [HttpDelete("/userRoadmapId/{userRoadmapId}/roadmapElementId/{roadmapElementId}")]
    public async Task<ActionResult> DeleteProcess(Guid userRoadmapId, Guid roadmapElementId)
    {
        Process? process = await unitOfWork.Processes.GetByIdAsync(userRoadmapId, roadmapElementId);
        if (process == null) return NotFound();

        // verify ownership to see, also checking for valid userRoadmapId
        UserRoadmap? userRoadmap = await unitOfWork.UserRoadmaps.GetByIdAsync(process.UserRoadmapId);
        if (userRoadmap == null ||
            currentUserService.UserId == null ||
            userRoadmap.UserId != currentUserService.UserId) return NotFound();

        // delete
        await unitOfWork.Processes.DeleteAsync(userRoadmapId, roadmapElementId);
        await unitOfWork.SaveChangesAsync();

        return NoContent();
    }



}
