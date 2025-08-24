using Application.Dtos.BookDto;
using Application.Dtos.VocabularyDto;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class VocabularyService : IVocabularyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VocabularyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddVocabularyAsync(VocabularyCreateDto vocabularyCreateDto)
        {
            var vocabulary = _mapper.Map<Vocabulary>(vocabularyCreateDto);
            await _unitOfWork.Vocabularies.AddAsync(vocabulary);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteVocabularyAsync(Guid id)
        {
            var vocabulary = await _unitOfWork.Vocabularies.GetByIdAsync(id);
            if (vocabulary == null)
            {
                throw new KeyNotFoundException("Vocabulary not found");
            }
            else
            {
                await _unitOfWork.Vocabularies.Delete(vocabulary);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<VocabularyDto>> GetAllVocabularyAsync()
        {
            var vocabularies = await _unitOfWork.Vocabularies.GetAllAsync();
            return _mapper.Map<IEnumerable<VocabularyDto>>(vocabularies);
        }

        public async Task<IEnumerable<VocabularyInBookDto>> GetVocabularyByBookIdAsync(Guid bookId)
        {
            var vocabularies = await _unitOfWork.Vocabularies.GetAllVocabulariesByBookIdAsync(bookId);
            return _mapper.Map<IEnumerable<VocabularyInBookDto>>(vocabularies);
        }

        public async Task<VocabularyDto> GetVocabularyByIdAsync(Guid id)
        {
            var vocabulary = await _unitOfWork.Vocabularies.GetByIdAsync(id);
            if (vocabulary == null)
            {
                throw new KeyNotFoundException("Vocabulary not found");
            }
            return _mapper.Map<VocabularyDto>(vocabulary);
        }

        public async Task UpdateVocabularyAsync(VocabularyUpdateDto vocabularyUpdateDto)
        {
            var vocaUpdate = await _unitOfWork.Vocabularies.GetByIdAsync(vocabularyUpdateDto.Id);
            if (vocaUpdate == null)
            {
                throw new KeyNotFoundException("Vocabulary not found");
            }
            else
            {
                var vocabulary = _mapper.Map<Vocabulary>(vocabularyUpdateDto);
                await _unitOfWork.Vocabularies.Update(vocabulary);
                var result = await _unitOfWork.SaveChangesAsync();
                return;
            }
        }
    }
}
