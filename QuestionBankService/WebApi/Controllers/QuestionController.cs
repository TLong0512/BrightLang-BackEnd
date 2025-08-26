using Application.Dtos.QuestionDtos;
using Application.Services;
using Application.Services.Intefaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IMapper _mapper;

        public QuestionController(IQuestionService QuestionService, IMapper mapper)
        {
            _questionService = QuestionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GettAllQuestion()
        {
            try
            {
                var result = await _questionService.GellAllQuestionAsync();
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
        [HttpGet("generate-question/{rangeId}")]
        public async Task<IActionResult> GenerateQuestionInARange(Guid rangeId)
        {
            try
            {
                var result = await _questionService.GenerateRandomQuestionInARangeByRangeId(rangeId);
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
        [HttpGet("question-in-context/{contextId}")]
        public async Task<IActionResult> GetQuestionByContextId(Guid contextId)
        {
            try
            {
                if (contextId == Guid.Empty)
                {
                    return BadRequest("Invalid parametters");
                }
                var result = await _questionService.GetQuestionsByContextIdAsync(contextId);
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
        public async Task<IActionResult> AddQuestion([FromBody] QuestionAddDto QuestionAddDto)
        {
            try
            {
                if (QuestionAddDto == null)
                { return BadRequest("Invalid data"); }
                var result = await _questionService.AddQuestionAsync(QuestionAddDto);
                if (result == Guid.Empty)
                {
                    return BadRequest();
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
        [HttpPost("quick-add/{skilllevelId}")]
        public async Task<IActionResult> QuickAddQuestion(Guid skilllevelId,[FromBody] List<QuickQuestionAddDto> quickQuestionAddDto)
        {
            try
            {
                if (quickQuestionAddDto == null)
                { return BadRequest("Invalid data"); }
                var result = await _questionService.QuickAddQuestion(skilllevelId, quickQuestionAddDto);
                if (result == false)
                {
                    return BadRequest();
                }
                return Ok("Added sucessfully");
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
        public async Task<IActionResult> GetQuestionById(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest();
                }
                var result = await _questionService.GetQuestionByIdAsync(id);
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
        public async Task<IActionResult> UpdateQuestion(Guid id, QuestionUpdateDto QuestionUpdateDto)
        {
            try
            {
                var result = await _questionService.UpdateQuestionAsync(id, QuestionUpdateDto);
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
        public async Task<IActionResult> DeleteQuestion(Guid id)
        {
            try
            {
                var deleted = await _questionService.DeleteQuestionAsync(id);

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
