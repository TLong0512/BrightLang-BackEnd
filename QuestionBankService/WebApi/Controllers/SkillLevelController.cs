using Application.Dtos.SkillLevelDto;
using Application.Services.Intefaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillLevelController : ControllerBase
    {
        private readonly ISkillLevelService _skillLevelService;
        private readonly IMapper _mapper;

        public SkillLevelController(ISkillLevelService SkillLevelService, IMapper mapper)
        {
            _skillLevelService = SkillLevelService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GettAllSkillLevel()
        {
            try
            {
                var result = await _skillLevelService.GellAllSkillLevelAsync();
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
        [HttpPost]
        public async Task<IActionResult> AddSkillLevel([FromBody] SkillLevelAddDto SkillLevelAddDto)
        {
            try
            {
                if (SkillLevelAddDto == null)
                { return BadRequest("Invalid data"); }
                var result = await _skillLevelService.AddSkillLevelAsync(SkillLevelAddDto);
                if(result == false)
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
        public async Task<IActionResult> GetSkillLevelById(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest("Invalid parametters");
                }
                var result = await _skillLevelService.GetSkillLevelByIdAsync(id);
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
        public async Task<IActionResult> UpdateSkillLevel(Guid id, SkillLevelUpdateDto SkillLevelUpdateDto)
        {
            try
            {
                var result = await _skillLevelService.UpdateSkillLevelAsync(id, SkillLevelUpdateDto);
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
        public async Task<IActionResult> DeleteSkillLevel(Guid id)
        {
            try
            {
                var deleted = await _skillLevelService.DeleteSkillLevelAsync(id);

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
