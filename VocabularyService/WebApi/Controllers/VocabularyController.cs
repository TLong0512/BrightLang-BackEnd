using Application.Dtos.VocabularyDto;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VocabularyController : ControllerBase
    {
        private readonly IVocabularyService _vocabularyService;

        public VocabularyController(IVocabularyService vocabularyService)
        {
            _vocabularyService = vocabularyService;
        }

        // GET: api/vocabulary
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> GetAllVocabulary([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var vocabularies = await _vocabularyService.GetAllVocabularyAsync(page, pageSize);
            return Ok(vocabularies);
        }

        // GET: api/vocabulary/{id}
        [Authorize(Roles = "User")]
        [HttpGet("{id}")]
        public async Task<ActionResult<VocabularyDto>> GetVocabularyById(Guid id)
        {
            try
            {
                var vocabulary = await _vocabularyService.GetVocabularyByIdAsync(id);
                return Ok(vocabulary);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/vocabulary/book/{bookId}
        [Authorize(Roles = "User")]
        [HttpGet("book/{bookId}")]
        public async Task<ActionResult> GetVocabularyByBookId(Guid bookId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var vocabularies = await _vocabularyService.GetVocabularyByBookIdAsync(bookId, page, pageSize);
            return Ok(vocabularies);
        }

        // POST: api/vocabulary
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<ActionResult> AddVocabulary([FromBody] VocabularyCreateDto vocabularyCreateDto)
        {
            await _vocabularyService.AddVocabularyAsync(vocabularyCreateDto);
            return Ok(new { message = "Vocabulary added successfully" });
        }

        // PUT: api/vocabulary
        [Authorize(Roles = "User")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateVocabulary(Guid id, [FromBody] VocabularyUpdateDto vocabularyUpdateDto)
        {
            try
            {
                await _vocabularyService.UpdateVocabularyAsync(vocabularyUpdateDto, id);
                return Ok(new { message = "Vocabulary updated successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/vocabulary/{id}
        [Authorize(Roles = "User")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVocabulary(Guid id)
        {
            try
            {
                await _vocabularyService.DeleteVocabularyAsync(id);
                return Ok(new { message = "Vocabulary deleted successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
