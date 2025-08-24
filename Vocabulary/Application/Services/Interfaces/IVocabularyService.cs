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
        Task<IEnumerable<VocabularyDto>> GetAllVocabularyAsync();
        Task<IEnumerable<VocabularyInBookDto>> GetVocabularyByBookIdAsync(Guid bookId);
        Task<VocabularyDto> GetVocabularyByIdAsync(Guid id);
        Task AddVocabularyAsync(VocabularyCreateDto vocabularyCreateDto);
        Task UpdateVocabularyAsync(VocabularyUpdateDto vocabularyUpdateDto);
        Task DeleteVocabularyAsync(Guid id);
    }
}
