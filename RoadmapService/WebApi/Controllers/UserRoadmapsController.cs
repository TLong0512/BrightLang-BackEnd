        using Application.Abstraction;
        using Application.Abstraction.Services;
        using Application.Dtos.BaseDtos;
        using Application.Dtos.ProcessDtos;
        using Application.Dtos.QuestionBankService;
        using Application.Dtos.RoadmapDtos;
        using Application.Dtos.UserRoadmapDtos;
        using Domain.Entities;
        using Microsoft.AspNetCore.Authorization;
        using Microsoft.AspNetCore.Mvc;

        namespace WebApi.Controllers;


        // used for user with their roadmaps.
        [Route("api/[controller]")]
        [ApiController]
        [Authorize(Roles = "User")]
        public class UserRoadmapsController : ControllerBase
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly IQuestionbankService questionbankService;
            private readonly ICurrentUserService currentUserService;

            public UserRoadmapsController(IUnitOfWork unitOfWork, IQuestionbankService questionbankService, ICurrentUserService currentUserService)
            {
                this.unitOfWork = unitOfWork;
                this.questionbankService = questionbankService;
                this.currentUserService = currentUserService;
            }

            [HttpGet]
            public async Task<ActionResult<PageResultDto<UserRoadmapGeneralDto>>> GetMyRoadmaps(
                [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
            {
                // get all of my roadmap
                Guid? userId = currentUserService.UserId;
                IEnumerable<UserRoadmap> allMyRoadmaps = await unitOfWork.UserRoadmaps
                    .FindAsync(r => r.UserId == userId);

                // paging
                IEnumerable<UserRoadmap> myRoadmaps = allMyRoadmaps.Skip((page - 1) * pageSize)
                    .Take(pageSize); // TODO: performance issue.

                // caching.
                // user roadmpa is built on given roadmap. they can be duplicate
                IEnumerable<Guid> usedRoadmapId = myRoadmaps.Select(r => r.RoadmapId).Distinct();
                IEnumerable<Roadmap> roadmapsIStudied = await unitOfWork.Roadmaps.FindAsync(r => usedRoadmapId.Contains(r.Id));

                // return list value
                List<UserRoadmapGeneralDto> returnItems = [];

                // build the list
                foreach (UserRoadmap myRoadmap in myRoadmaps)
                {
                    Roadmap theRoadmap = roadmapsIStudied.SingleOrDefault(r => r.Id == myRoadmap.RoadmapId)
                        ?? new Roadmap { Name = "Unknown roadmap." };

                    var processes = await unitOfWork.Processes.FindAsync(r => r.UserRoadmapId == myRoadmap.Id);
                    ProcessStatisticDto processStatistic = new ProcessStatisticDto();
                    if (processes.Any() == true)
                    {
                        processStatistic = new ProcessStatisticDto
                        {
                            AllProcessCount = processes.Count(),
                            FinishedProcessCount = processes.Where(r => r.IsFinished == true).Count(),
                            OpenedProcessCount = processes.Where(r => r.IsOpened == true).Count(),
                        };
                    }

                    UserRoadmapGeneralDto userRoadmapDetailDto = new UserRoadmapGeneralDto
                    {
                        Id = myRoadmap.Id,
                        Roadmap = new RoadmapGeneralDto
                        {
                            Id = theRoadmap.Id,
                            Name = theRoadmap.Name,
                            LevelStart = await questionbankService.GetLevelByIdAsync(theRoadmap.LevelStartId)
                                        ?? new LevelViewDto { Name = "Unknown start level." },
                            LevelEnd = await questionbankService.GetLevelByIdAsync(theRoadmap.LevelEndId)
                                        ?? new LevelViewDto { Name = "Unknown end level." },
                        },
                        ProcessStatistic = processStatistic,
                    };
                    returnItems.Add(userRoadmapDetailDto);
                }

                // return
                return new PageResultDto<UserRoadmapGeneralDto>
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalItem = allMyRoadmaps.Count(),
                    Items = returnItems
                };
            }


            [HttpGet("{id:guid}")]
            public async Task<ActionResult<UserRoadmapDetailDto>> GetMyRoadmap(Guid id)
            {
                Guid? userId = currentUserService.UserId;
                IEnumerable<UserRoadmap> allMyRoadmaps = await unitOfWork.UserRoadmaps
                    .FindAsync(r => r.UserId == userId && r.Id == id);

                if (allMyRoadmaps.Any() == false) return NotFound(); // FE error

                UserRoadmap myRoadmap = allMyRoadmaps.Single();
                Roadmap? theUsedRoadmap = await unitOfWork.Roadmaps.GetByIdAsync(myRoadmap.RoadmapId)
                    ?? new Roadmap { Name = "Unknown roadmap." };

                IEnumerable<Process> processes = await unitOfWork.Processes
                    .FindAsync(e => e.UserRoadmapId == myRoadmap.Id);

                IEnumerable<RoadmapElement> roadmapElements = await unitOfWork.RoadmapElements
                    .FindAsync(e => e.RoadmapId == theUsedRoadmap.Id);

                IEnumerable<ProcessDetailDto> compiledProcesses = processes.Zip(
                    roadmapElements, (process, roadmapElement) => new ProcessDetailDto
                    {
                        RoadmapElementId = roadmapElement.Id,
                        QuestionPerDay = roadmapElement.QuestionPerDay,
                        RangeId = roadmapElement.RangeId,
                        IsFinished = process.IsFinished,
                        IsOpened = process.IsOpened,
                    });

                return Ok(new UserRoadmapDetailDto
                {
                    Id = myRoadmap.Id,
                    Roadmap = new RoadmapPreviewDto
                    {
                        Id = theUsedRoadmap.Id,
                        Name = theUsedRoadmap.Name,
                    },
                    Processs = compiledProcesses.ToList(),
                });
            }


            [HttpPost]
            public async Task<ActionResult<UserRoadmapGeneralDto>> CreateMyRoadmap([FromBody] UserRoadmapPostDto userRoadmapPostDto)
            {
                Roadmap? existRoadmap = await unitOfWork.Roadmaps.GetByIdAsync(userRoadmapPostDto.RoadmapId);
                if (existRoadmap == null) return NotFound(); // FE error

                UserRoadmap myNewUserRoadmap = new UserRoadmap
                {
                    UserId = currentUserService.UserId!.Value,
                    RoadmapId = userRoadmapPostDto.RoadmapId,
                };
                await unitOfWork.UserRoadmaps.AddAsync(myNewUserRoadmap);
                await unitOfWork.SaveChangesAsync(); // doesnt ge tthe exception

                // add processes
                List<RoadmapElement> roadmapElements = (await unitOfWork.RoadmapElements
                    .FindAsync(r => r.RoadmapId == existRoadmap.Id))
                    .ToList();

                List<Process> processes = new List<Process>();
                bool isFirst = true;
                foreach (RoadmapElement element in roadmapElements)
                {
                    var process = new Process
                    {
                        UserRoadmapId = myNewUserRoadmap.Id,
                        RoadmapElementId = element.Id,
                        IsFinished = false,
                        IsOpened = false,
                    };
                    if(isFirst)
                    {
                        isFirst = false;
                        process.IsOpened = true;
                    }
                    await unitOfWork.Processes.AddAsync(process);
                    processes.Add(process);
                }
                await unitOfWork.SaveChangesAsync(); // got exceptions StartDate cant be null
                // end of add processes

                IEnumerable<ProcessDetailDto> compiledProcesses = processes.Zip(
                    roadmapElements, (process, roadmapElement) => new ProcessDetailDto
                    {
                        RoadmapElementId = roadmapElement.Id,
                        QuestionPerDay = roadmapElement.QuestionPerDay,
                        RangeId = roadmapElement.RangeId,
                        IsFinished = process.IsFinished,
                        IsOpened = process.IsOpened,
                    });

                return CreatedAtAction(nameof(GetMyRoadmap), new { id = myNewUserRoadmap.Id }, new UserRoadmapDetailDto
                {
                    Id = myNewUserRoadmap.Id,
                    Roadmap = new RoadmapPreviewDto
                    {
                        Id = existRoadmap.Id,
                        Name = existRoadmap.Name,
                    },
                    Processs = compiledProcesses.ToList(),
                });
            }

            // UserRoadmap is not meant to be changed
            // but UserRoadmap's RoadmapElement can.
            // we implements PUT endpoints to toggle fields.
            //[HttpPut("{userRoadmapId}/roadmapElement/{roadmapElementId}/toggle-finish")]

            //public async Task<ActionResult> ToggleProcessIsFinished(Guid userRoadmapId, Guid roadmapElementId)
            //{
            //    Process? process = await unitOfWork.Processes.GetByIdAsync(userRoadmapId, roadmapElementId);
            //    if (process == null) return NotFound();

            //    // verify ownership to see, also checking for valid userRoadmapId
            //    UserRoadmap? userRoadmap = await unitOfWork.UserRoadmaps.GetByIdAsync(process.UserRoadmapId);
            //    if (userRoadmap == null ||
            //        currentUserService.UserId == null ||
            //        userRoadmap.UserId != currentUserService.UserId) return NotFound();

            //    // toggle
            //    process.IsFinished = !process.IsFinished;
            //    await unitOfWork.SaveChangesAsync();

            //    return Ok(process);
            //}

            [HttpPut("{userRoadmapId:guid}/{order:int}/toggle-finish")]
            public async Task<ActionResult> ToggleProcessIsFinished(Guid userRoadmapId, int order)
            {
                // verify ownership to see, also checking for valid userRoadmapId
                UserRoadmap? userRoadmap = await unitOfWork.UserRoadmaps.GetByIdAsync(userRoadmapId);
                if (userRoadmap == null ||
                    currentUserService.UserId == null ||
                    userRoadmap.UserId != currentUserService.UserId) return NotFound();

                Roadmap? roadmap = await unitOfWork.Roadmaps.GetByIdAsync(userRoadmap.RoadmapId);
                if (roadmap == null) return NotFound();

                IEnumerable<RoadmapElement> roadmapElements = await unitOfWork.RoadmapElements.FindAsync(r => r.RoadmapId == roadmap.Id && r.Order == order);
                if (roadmapElements.Any() == false) return NotFound();
                RoadmapElement roadmapElement = roadmapElements.ElementAt(0);

                IEnumerable<Process> processes = await unitOfWork.Processes.FindAsync(r => r.UserRoadmapId == userRoadmapId && r.RoadmapElementId == roadmapElement.Id);
                if (processes.Any() == false) return NotFound();
                Process process = processes.ElementAt(0);

                // toggle
                process.IsFinished = !process.IsFinished;
                await unitOfWork.SaveChangesAsync();

                return Ok();
            }

            //[HttpPut("{userRoadmapId:guid}/roadmapElement/{roadmapElementId:guid}/toggle-open")]
            //public async Task<ActionResult> ToggleProcessIsOpened(Guid userRoadmapId, Guid roadmapElementId)
            //{
            //    Process? process = await unitOfWork.Processes.GetByIdAsync(userRoadmapId, roadmapElementId);
            //    if (process == null) return NotFound();

            //    // verify ownership to see, also checking for valid userRoadmapId
            //    UserRoadmap? userRoadmap = await unitOfWork.UserRoadmaps.GetByIdAsync(process.UserRoadmapId);
            //    if (userRoadmap == null ||
            //        currentUserService.UserId == null ||
            //        userRoadmap.UserId != currentUserService.UserId) return NotFound();

            //    // toggle
            //    process.IsOpened = !process.IsOpened;
            //    await unitOfWork.SaveChangesAsync();

            //    return Ok(process);
            //}

            [HttpPut("{userRoadmapId:guid}/{order:int}/toggle-open")]
            public async Task<ActionResult> ToggleProcessIsOpened(Guid userRoadmapId, int order)
            {
                // verify ownership to see, also checking for valid userRoadmapId
                UserRoadmap? userRoadmap = await unitOfWork.UserRoadmaps.GetByIdAsync(userRoadmapId);
                if (userRoadmap == null ||
                    currentUserService.UserId == null ||
                    userRoadmap.UserId != currentUserService.UserId) return NotFound();

                Roadmap? roadmap = await unitOfWork.Roadmaps.GetByIdAsync(userRoadmap.RoadmapId);
                if (roadmap == null) return NotFound();

                IEnumerable<RoadmapElement> roadmapElements = await unitOfWork.RoadmapElements.FindAsync(r => r.RoadmapId == roadmap.Id && r.Order == order);
                if (roadmapElements.Any() == false) return NotFound();
                RoadmapElement roadmapElement = roadmapElements.ElementAt(0);

                IEnumerable<Process> processes = await unitOfWork.Processes.FindAsync(r => r.UserRoadmapId == userRoadmapId && r.RoadmapElementId == roadmapElement.Id);
                if (processes.Any() == false) return NotFound();
                Process process = processes.ElementAt(0);

                // toggle
                process.IsOpened = !process.IsOpened;
                await unitOfWork.SaveChangesAsync();

                return Ok();
            }


            // if user want to enroll a new roadmap, 
            // just make a new userroadmap and obsolete this. or delete this userroadmap, if they are bothered.

            [HttpDelete("{id:guid}")]
            public async Task<IActionResult> DeleteRoadmap(Guid id)
            {
                Guid userId = currentUserService.UserId!.Value;
                IEnumerable<UserRoadmap> roadmaps = await unitOfWork.UserRoadmaps
                    .FindAsync(r => r.Id == id && r.UserId == userId);

                if (roadmaps.Any() == false) return NotFound();

                // delete associated processes
                IEnumerable<Process> processes = await unitOfWork.Processes.FindAsync(r => r.UserRoadmapId == id);
                foreach (Process process in processes)
                {
                    await unitOfWork.Processes.DeleteAsync(id, process.RoadmapElementId);
                }

                // delete the main thing
                await unitOfWork.UserRoadmaps.DeleteAsync(id);
                await unitOfWork.SaveChangesAsync();

                return NoContent();
            }
        }
