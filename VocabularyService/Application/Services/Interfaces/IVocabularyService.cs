using Application.Dtos.BaseDto;
using Application.Dtos.VocabularyDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IVocabularyService
    {
        Task<PageResultDto<VocabularyDto>> GetAllVocabularyAsync(int page, int pageSize);
        Task<PageResultDto<VocabularyInBookDto>> GetVocabularyByBookIdAsync(Guid bookId, int page, int pageSize);
        Task<VocabularyDto> GetVocabularyByIdAsync(Guid id);
        Task AddVocabularyAsync(VocabularyCreateDto vocabularyCreateDto);
        Task UpdateVocabularyAsync(VocabularyUpdateDto vocabularyUpdateDto, Guid id);
        Task DeleteVocabularyAsync(Guid id);
    }
}
