using Application.Dtos.RangeDtos;
using Application.Services.Implementations;
using Application.Services.Intefaces;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RangeController : ControllerBase
    {
        private readonly IRangeService _rangeService;
        private readonly IMapper _mapper;

        public RangeController(IRangeService RangeService, IMapper mapper)
        {
            _rangeService = RangeService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GettAllRange()
        {
            try
            {
                var result = await _rangeService.GellAllRangeAsync();
                if (result == null || !result.Any())
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

        [HttpGet("range-in-skill-level/{skillLevelId}")]
        public async Task<IActionResult> GetAllRangeInSkillLevelBySkillLevelId(Guid skillLevelId)
        {
            try
            {
                if (skillLevelId == Guid.Empty)
                {
                    return BadRequest("Invalid parametters");
                }
                var result = await _rangeService.GetRangesBySkillLevelIdAsync(skillLevelId);
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
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRange([FromBody] RangeAddDto RangeAddDto)
        {
            try
            {
                if (RangeAddDto == null)
                { return BadRequest("Invalid data"); }
                var result = await _rangeService.AddRangeAsync(RangeAddDto);
                if (result == false)
                {
                    return BadRequest();
                }
                return Ok("Add successfully");
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
        public async Task<IActionResult> GetRangeById(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest("Invalid parametters");
                }
                var result = await _rangeService.GetRangeByIdAsync(id);
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
        public async Task<IActionResult> UpdateRange(Guid id, RangeUpdateDto RangeUpdateDto)
        {
            try
            {
                var result = await _rangeService.UpdateRangeAsync(id, RangeUpdateDto);
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
        public async Task<IActionResult> DeleteRange(Guid id)
        {
            try
            {
                var deleted = await _rangeService.DeleteRangeAsync(id);

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
