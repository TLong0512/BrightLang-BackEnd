using Application.Dtos.ExamTypeDtos;
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
    public class ExamTypeController : ControllerBase
    {
        private readonly IExamTypeService _examTypeService;
        private readonly IMapper _mapper;

        public ExamTypeController(IExamTypeService examTypeService, IMapper mapper)
        {
            _examTypeService = examTypeService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GettAllExamType()
        {
            try
            {
                var result = await _examTypeService.GellAllExamTypeAsync();
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
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddExamType([FromBody] ExamTypeAddDto examTypeAddDto)
        {
            try
            {
                if (examTypeAddDto == null)
                { return BadRequest("Invalid data"); }
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("UserId not found in token");

                Guid userId = Guid.Parse(userIdClaim);

                await _examTypeService.AddExamTypeAsync(examTypeAddDto, userId);
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
        public async Task<IActionResult> GetExamTypeById(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest("Invalid parametters");
                }
                var result = await _examTypeService.GetExamTypeByIdAsync(id);
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
        public async Task<IActionResult> UpdateExamType(Guid id, ExamTypeUpdateDto examTypeUpdateDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("UserId not found in token");

                Guid userId = Guid.Parse(userIdClaim);

                var result = await _examTypeService.UpdateExamTypeAsync(id, examTypeUpdateDto, userId);
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
        public async Task<IActionResult> DeleteExamType(Guid id)
        {
            try
            {
                var deleted = await _examTypeService.DeleteExamTypeAsync(id);

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
