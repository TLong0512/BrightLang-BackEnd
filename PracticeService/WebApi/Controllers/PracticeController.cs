using Application.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PracticeController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public PracticeController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet("examtypes")]
        public async Task<IActionResult> GetExamTypes()
        {
            var examTypes = await _questionService.GetAllExamTypesAsync();
            return Ok(examTypes);
        }

        [HttpGet("levels/{examTypeId}")]
        public async Task<IActionResult> GetLevelsByExamTypeId(Guid examTypeId)
        {
            if (examTypeId == Guid.Empty)
            {
                return BadRequest("Invalid exam type id.");
            }

            var levels = await _questionService.GetLevelsByExamTypeIdAsync(examTypeId);
            return Ok(levels);
        }

        [HttpGet("skillLevels/{levelId}")]
        public async Task<IActionResult> GetSkillsByLevelId(Guid levelId)
        {
            if (levelId == Guid.Empty)
            {
                return BadRequest("Invalid level id.");
            }

            var questions = await _questionService.GetSkillLevelByLevelIdAsync(levelId);
            if (!questions.Any())
            {
                return NotFound("No questions found.");
            }

            return Ok(questions);
        }

        [HttpGet("ranges/{skillLevelId}")]
        public async Task<IActionResult> GetRangesBySkillLevel(Guid skillLevelId)
        {
            if (skillLevelId == Guid.Empty)
            {
                return BadRequest("Invalid skill level id.");
            }

            var ranges = await _questionService.GetRangesBySkillLevelIdAsync(skillLevelId);
            if (!ranges.Any())
            {
                return NotFound("No ranges found.");
            }

            return Ok(ranges);
        }

        [HttpGet("generate/{rangeId}")]
        public async Task<IActionResult> GenerateQuestions(Guid rangeId)
        {
            try
            {
                var result = await _questionService.GenerateQuestionsByRangeIdAsync(rangeId);
                if (!result.Any())
                    return NotFound("No questions found");

                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("detail/{rangeId}")]
        public async Task<IActionResult> ShowDetailQuestions(Guid rangeId)
        {
            if (rangeId == Guid.Empty)
                return BadRequest("Invalid range id.");

            var result = await _questionService.ShowDetailQuestionsByRangeIdAsync(rangeId);
            if (!result.Any())
                return NotFound("No questions found for this range.");

            return Ok(result);
        }
    }
}
