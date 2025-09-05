using Application.Abstraction.Services;
using Application.Abstraction;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Dtos.RoadmapElementRangeDtos;
using Application.Dtos.QuestionBankService;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoadmapElementRangesController : ControllerBase
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IQuestionbankService questionbankService;

    public RoadmapElementRangesController(IUnitOfWork unitOfWork, IQuestionbankService questionbankService)
    {
        this.unitOfWork = unitOfWork;
        this.questionbankService = questionbankService;
    }

    [HttpGet("{roadmapId:guid}")]
    public async Task<ActionResult<List<RoadmapElementRangeDto>>> GetRoadmapElements(Guid roadmapId)
    {
        Roadmap? roadmap = await unitOfWork.Roadmaps.GetByIdAsync(roadmapId);
        if (roadmap == null) return NotFound();

        List<RoadmapElement> currentRoadmapElements = (await unitOfWork.RoadmapElements
            .FindAsync(r => r.RoadmapId == roadmapId))
            .OrderBy(r => r.Order)
            .ToList();

        return await GenerateRoadmapElementSnapshot(currentRoadmapElements);
    }

    private async Task<List<RoadmapElementRangeDto>> GenerateRoadmapElementSnapshot(List<RoadmapElement> currentRoadmapElements)
    {
        List<RoadmapElementRangeDto> list = new List<RoadmapElementRangeDto>();

        // Run-Length Encoding (RLE)
        int count = 1;
        for (int i = 1; i < currentRoadmapElements.Count; i++)
        {
            RoadmapElement previous = currentRoadmapElements[i - 1];
            RoadmapElement current = currentRoadmapElements[i];

            bool isMatch = current.QuestionPerDay == previous.QuestionPerDay
                           && current.RangeId == previous.RangeId;

            if (isMatch)
            {
                count++;
            }
            else
            {
                list.Add(new RoadmapElementRangeDto
                {
                    Range = await questionbankService.GetRangeByIdAsync(previous.RangeId)
                            ?? new RangeViewDto { Name = "Unknown range" },
                    QuestionPerDay = previous.QuestionPerDay,
                    RepeatDays = count
                });
                count = 1;
            }
        }

        // flush the last group
        RoadmapElement last = currentRoadmapElements.Last();
        list.Add(new RoadmapElementRangeDto
        {
            Range = await questionbankService.GetRangeByIdAsync(last.RangeId)
                            ?? new RangeViewDto { Name = "Unknown range" },
            QuestionPerDay = last.QuestionPerDay,
            RepeatDays = count
        });
        // End of RLE

        return list;
    }

    //[HttpPost("{roadmapId:guid}/{index:int}")]
    //public async Task<ActionResult<List<RoadmapElementSnapshotDto>>> AddRoadmapElements(
    //    Guid roadmapId, int index, [FromBody] RoadmapElementSnapshotDto dto)
    //{
    //    Roadmap? roadmap = await unitOfWork.Roadmaps.GetByIdAsync(roadmapId);
    //    if (roadmap == null) return NotFound();

    //    await AddRangeAsync(roadmap.Id, index, dto);

    //    List<RoadmapElement> currentRoadmapElements = (await unitOfWork.RoadmapElements
    //        .FindAsync(r => r.RoadmapId == roadmapId))
    //        .OrderBy(r => r.Order)
    //        .ToList();

    //    return GenerateRoadmapElementSnapshot(currentRoadmapElements);
    //}

    [Authorize(Roles = "Admin")]
    [HttpPut("{roadmapId:guid}/{index:int}")]
    public async Task<ActionResult<List<RoadmapElementRangeDto>>> UpdateRoadmapElements(
        Guid roadmapId, int index,
        [FromBody] RoadmapElementRangeUpdateDto dto)
    {
        Roadmap? roadmap = await unitOfWork.Roadmaps.GetByIdAsync(roadmapId);
        if (roadmap == null) return NotFound();

        await UpdateRangeAsync(roadmap.Id, index, dto);

        List<RoadmapElement> currentRoadmapElements = (await unitOfWork.RoadmapElements
            .FindAsync(r => r.RoadmapId == roadmapId))
            .OrderBy(r => r.Order)
            .ToList();

        return await GenerateRoadmapElementSnapshot(currentRoadmapElements);
    }

    //[HttpDelete("{roadmapId:guid}/{index:int}")]
    //public async Task<ActionResult<List<RoadmapElementSnapshotDto>>> DeleteRoadmapElements(
    //    Guid roadmapId, int index)
    //{
    //    Roadmap? roadmap = await unitOfWork.Roadmaps.GetByIdAsync(roadmapId);
    //    if (roadmap == null) return NotFound();

    //    await DeleteRangeAsync(roadmap.Id, index);

    //    List<RoadmapElement> currentRoadmapElements = (await unitOfWork.RoadmapElements
    //        .FindAsync(r => r.RoadmapId == roadmapId))
    //        .OrderBy(r => r.Order)
    //        .ToList();

    //    return GenerateRoadmapElementSnapshot(currentRoadmapElements);
    //}


    private async Task DeleteRangeAsync(Guid roadmapId, int snapshotIndex)
    {
        // get the source
        List<RoadmapElement> roadmapElements = (await unitOfWork.RoadmapElements
            .FindAsync(r => r.RoadmapId == roadmapId))
            .OrderBy(r => r.Order)
            .ToList();

        // Build snapshot
        var snapshots = await GenerateRoadmapElementSnapshot(roadmapElements);

        if (snapshotIndex < 0 || snapshotIndex >= snapshots.Count)
            throw new ArgumentOutOfRangeException(nameof(snapshotIndex), "Invalid snapshot index");

        // Find start position in expanded list
        int startIndex = snapshots.Take(snapshotIndex).Sum(s => s.RepeatDays);
        int count = snapshots[snapshotIndex].RepeatDays;

        // list to delete
        var deletes = roadmapElements.Skip(startIndex).Take(count).ToList();

        // list to update the order
        var updates = roadmapElements.Skip(startIndex + count).ToList();

        // Remove the block
        foreach (var elem in deletes)
        {
            await unitOfWork.RoadmapElements.DeleteAsync(elem.Id);
        }

        // Recalculate orders
        for (int i = 0; i < updates.Count; i++)
        {
            updates[i].Order = startIndex + i + 1;
            await unitOfWork.RoadmapElements.UpdateAsync(updates[i]);
        }

        await unitOfWork.SaveChangesAsync();
    }

    private Task AddRangeAsync(Guid roadmapId, int snapshotIndex, RoadmapElementRangeUpdateDto snapshot)
    {
        throw new NotImplementedException();

        //// get the source
        //List<RoadmapElement> roadmapElements = (await unitOfWork.RoadmapElements
        //    .FindAsync(r => r.RoadmapId == roadmapId))
        //    .OrderBy(r => r.Order)
        //    .ToList();

        //// Build snapshot
        //var snapshots = GenerateRoadmapElementSnapshot(roadmapElements);

        //if (snapshotIndex < 0 || snapshotIndex > snapshots.Count)
        //    throw new ArgumentOutOfRangeException(nameof(snapshotIndex), "Invalid snapshot index");

        //// Find start position in expanded list
        //int insertIndex = snapshots.Take(snapshotIndex).Sum(s => s.RepeatDays);

        //// Build new elements to insert
        //List<RoadmapElement> newElements = new List<RoadmapElement>();
        //for (int i = 0; i < snapshot.RepeatDays; i++)
        //{
        //    newElements.Add(new RoadmapElement
        //    {
        //        Id = Guid.NewGuid(),
        //        RoadmapId = roadmapId,
        //        RangeId = snapshot.RangeId,
        //        QuestionPerDay = snapshot.QuestionPerDay,
        //        Order = insertIndex + i + 1
        //    });
        //}

        //// Shift following elements
        //var updates = roadmapElements.Skip(insertIndex).ToList();
        //for (int i = 0; i < updates.Count; i++)
        //{
        //    updates[i].Order = insertIndex + snapshot.RepeatDays + i + 1;
        //    await unitOfWork.RoadmapElements.UpdateAsync(updates[i]);
        //}

        //// Save inserts
        //foreach (var elem in newElements)
        //{
        //    await unitOfWork.RoadmapElements.AddAsync(elem);
        //}

        //await unitOfWork.SaveChangesAsync();
    }

    private async Task UpdateRangeAsync(Guid roadmapId, int snapshotIndex, RoadmapElementRangeUpdateDto snapshot)
    {
        // get the source
        List<RoadmapElement> roadmapElements = (await unitOfWork.RoadmapElements
            .FindAsync(r => r.RoadmapId == roadmapId))
            .OrderBy(r => r.Order)
            .ToList();

        // Build snapshot
        var snapshots = await GenerateRoadmapElementSnapshot(roadmapElements);

        if (snapshotIndex < 0 || snapshotIndex >= snapshots.Count)
            throw new ArgumentOutOfRangeException(nameof(snapshotIndex), "Invalid snapshot index");

        // Find start position in expanded list
        int startIndex = snapshots.Take(snapshotIndex).Sum(s => s.RepeatDays);
        int oldCount = snapshots[snapshotIndex].RepeatDays;
        int newCount = snapshot.RepeatDays;

        // get block to update
        var block = roadmapElements.Skip(startIndex).Take(oldCount).ToList();

        if (block.Count == 0)
            throw new InvalidOperationException("Snapshot block not found in elements");

        //// --- CASE 1: Different RangeId → replace whole block ---
        //if (block.First().RangeId != snapshot.RangeId)
        //{
        //    // delete old
        //    foreach (var elem in block)
        //        await unitOfWork.RoadmapElements.DeleteAsync(elem.Id);

        //    // add new
        //    for (int i = 0; i < newCount; i++)
        //    {
        //        var elem = new RoadmapElement
        //        {
        //            Id = Guid.NewGuid(),
        //            RoadmapId = roadmapId,
        //            RangeId = snapshot.RangeId,
        //            QuestionPerDay = snapshot.QuestionPerDay,
        //            Order = startIndex + i + 1
        //        };
        //        await unitOfWork.RoadmapElements.AddAsync(elem);
        //    }
        //}
        //else
        //{
        // --- CASE 2: Same RangeId → preserve as much as possible ---
        int minCount = Math.Min(oldCount, newCount);

        // update existing ones
        for (int i = 0; i < minCount; i++)
        {
            block[i].QuestionPerDay = snapshot.QuestionPerDay;
            block[i].Order = startIndex + i + 1;
            await unitOfWork.RoadmapElements.UpdateAsync(block[i]);
        }

        // if new is longer → add more at end
        if (newCount > oldCount)
        {
            List<RoadmapElement> newRoadmapElements = new List<RoadmapElement>();
            for (int i = oldCount; i < newCount; i++)
            {
                var elem = new RoadmapElement
                {
                    Id = Guid.NewGuid(),
                    RoadmapId = roadmapId,
                    //RangeId = snapshot.RangeId,
                    RangeId = block.First().RangeId,
                    QuestionPerDay = snapshot.QuestionPerDay,
                    Order = startIndex + i + 1
                };
                await unitOfWork.RoadmapElements.AddAsync(elem);
                newRoadmapElements.Add(elem);
                }

            // and add new process for new user who enrolls them
            List<UserRoadmap> userRoadmaps = (await unitOfWork.UserRoadmaps
                .FindAsync(r => r.RoadmapId == roadmapId))
                .ToList();

            foreach(var userRoadmap in userRoadmaps)
            {
                foreach (var roadmapElement in newRoadmapElements)
                {
                    await unitOfWork.Processes.AddAsync(new Process
                    {
                        UserRoadmapId = userRoadmap.Id,
                        RoadmapElementId = roadmapElement.Id,
                        IsFinished = false,
                        IsOpened = false,
                    });
                }
            }
            await unitOfWork.SaveChangesAsync();
        }
        // if new is shorter → delete from end
        else if (newCount < oldCount)
        {
            for (int i = newCount; i < oldCount; i++)
            {
                await unitOfWork.RoadmapElements.DeleteAsync(block[i].Id);
            }
            // on delete cascade will handle automatically.
        }
        //}

        // --- Fix orders of elements after this block ---
        var updates = roadmapElements.Skip(startIndex + oldCount).ToList();
        int shift = newCount - oldCount; // how much the length changed

        for (int i = 0; i < updates.Count; i++)
        {
            updates[i].Order = startIndex + newCount + i + 1;
            await unitOfWork.RoadmapElements.UpdateAsync(updates[i]);
        }

        await unitOfWork.SaveChangesAsync();
    }

}
