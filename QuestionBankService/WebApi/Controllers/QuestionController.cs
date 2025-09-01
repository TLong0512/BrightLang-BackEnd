using Application.Dtos.QuestionDtos;
using Application.Services;
using Application.Services.Intefaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

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
        public async Task<IActionResult> GetAllQuestion()
        {
            try
            {
                var result = await _questionService.GellAllQuestionAsync();
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
        [HttpGet("generate/range/{rangeId}")]
        [Authorize]
        public async Task<IActionResult> GenerateQuestionByRangeId(Guid rangeId)
        {
            try
            {
                if (rangeId == Guid.Empty)
                {
                    return BadRequest();
                }
                var result = await _questionService.GenerateQuestionByRangeIdAsync(rangeId);
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
        [HttpGet("generate/skill-level/{skillLevelId}/{number?}")]
        [Authorize]
        public async Task<IActionResult> GenerateQuestionBySkillLevelId(Guid skillLevelId, int number = 2)
        {
            try
            {
                if (skillLevelId == Guid.Empty)
                {
                    return BadRequest();
                }
                var result = await _questionService.GenerateQuestionBySkillLevelIdAsync(skillLevelId);
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
        [HttpGet("generate/level/{levelId}/{numberPerSkillLevel?}")]
        [Authorize]
        public async Task<IActionResult> GenerateQuestionByLevelId(Guid levelId, int numberPerSkillLevel = 2)
        {
            try
            {
                if(levelId == Guid.Empty)
                {
                    return BadRequest();
                }    
                var result = await _questionService.GenerateQuestionByLevelIdAsync(levelId, numberPerSkillLevel);
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
        [HttpGet("generate/examType/{examTypeId}/{numberPerSkillLevel?}")]
        [Authorize]
        public async Task<IActionResult> GenerateQuestionByExamTypeId(Guid examTypeId, int numberPerSkillLevel = 2)
        {
            try
            {
                if (examTypeId == Guid.Empty)
                {
                    return BadRequest();
                }
                var result = await _questionService.GenerateQuestionByExamTypeIdAsync(examTypeId, numberPerSkillLevel);
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
        [HttpGet("generate/all/{numberPerSkillLevel?}")]
        public async Task<IActionResult> GenerateAllQuestions(int numberPerSkillLevel = 2)
        {
            try
            {
                var result = await _questionService.GenerateAllExamTypeQuestionAsync(numberPerSkillLevel);
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
        [HttpGet("filter/context/{contextId}")]
        [Authorize]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddQuestion([FromBody] QuestionAddDto QuestionAddDto)
        {
            try
            {
                if (QuestionAddDto == null)
                { return BadRequest("Invalid data"); }

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("UserId not found in token");

                Guid userId = Guid.Parse(userIdClaim);

                var result = await _questionService.AddQuestionAsync(QuestionAddDto, userId);
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
        [HttpPost("quick-add/skill/{skillId}/exam-type/{examTypeId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> QuickAddQuestion(Guid skillId, Guid examTypeId,  [FromBody] List<QuickQuestionAddDto> quickQuestionAddDto)
        {
            try
            {
                if (quickQuestionAddDto == null)
                { return BadRequest("Invalid data"); }

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("UserId not found in token");

                Guid userId = Guid.Parse(userIdClaim);

                var result = await _questionService.QuickAddQuestion(skillId, examTypeId,quickQuestionAddDto, userId);
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
        [Authorize]
        public async Task<IActionResult> GetQuestionById(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest();
                }
                var result = await _questionService.GetQuestionDetailByIdAsync(id);
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
        public async Task<IActionResult> UpdateQuestion(Guid id, QuestionUpdateDto QuestionUpdateDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("UserId not found in token");

                Guid userId = Guid.Parse(userIdClaim);
                var result = await _questionService.UpdateQuestionAsync(id, QuestionUpdateDto, userId);
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

        [HttpPost("get-by-list/summary")]
        [Authorize]
        public async Task<IActionResult> GetListSummaryQuestions([FromBody]List<Guid> listQuestionIds)
        {
            try
            {
                if (!listQuestionIds.Any())
                {
                    return BadRequest();
                }
                var result = await _questionService.GetAllQuestionSummaryByListIdAsync(listQuestionIds);
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

        [HttpPost("get-by-list/detail")]
        [Authorize]
        public async Task<IActionResult> GetListDetailQuestions([FromBody]List<Guid> listQuestionIds)
        {
            try
            {
                if (!listQuestionIds.Any())
                {
                    return BadRequest();
                }
                var result = await _questionService.GetAllQuestionDetailByListIdAsync(listQuestionIds);
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
    }
}
