using Application.Dtos.LevelDtos;
using Application.Services.Intefaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LevelController : ControllerBase
    {
        private readonly ILevelService _levelService;
        private readonly IMapper _mapper;

        public LevelController(ILevelService LevelService, IMapper mapper)
        {
            _levelService = LevelService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GettAllLevel()
        {
            try
            {
                var result = await _levelService.GellAllLevelAsync();
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("filter/exam-type/{examTypeId}")]
        public async Task<IActionResult> GetAllLevelsByExamTypeIds(Guid examTypeId)
        {
            try
            {
                if (examTypeId == Guid.Empty)
                {
                    return BadRequest("Invalid exam type id.");
                }
                var result = await _levelService.GetLevelByExamTypeId(examTypeId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddLevel([FromBody] LevelAddDto LevelAddDto)
        {
            try
            {
                if (LevelAddDto == null)
                { return BadRequest("Invalid data"); }

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("UserId not found in token");

                Guid userId = Guid.Parse(userIdClaim);

                var result = await _levelService.AddLevelAsync(LevelAddDto, userId);
                if(result == false)
                {
                    return BadRequest();
                }
                return Ok(new {Message = "Add successfully" });
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLevelById(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest("Invalid parametters");
                }
                var result = await _levelService.GetLevelByIdAsync(id);
                if (result == null)
                {
                    return NotFound("No result found");
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateLevel(Guid id, LevelUpdateDto LevelUpdateDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("UserId not found in token");

                Guid userId = Guid.Parse(userIdClaim);
                var result = await _levelService.UpdateLevelAsync(id, LevelUpdateDto, userId);
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLevel(Guid id)
        {
            try
            {
                var deleted = await _levelService.DeleteLevelAsync(id);

                if (!deleted)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
