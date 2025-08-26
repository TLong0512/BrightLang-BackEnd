using Application.Dtos.BaseDto;
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
        private readonly ICurrentUserService _currentUserService;

        public VocabularyService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task AddVocabularyAsync(VocabularyCreateDto vocabularyCreateDto)
        {
            Guid? userIdToken = _currentUserService.UserId;

            var userId = _unitOfWork.Vocabularies.GetUserIdByBookId(vocabularyCreateDto.BookId);

            if (userIdToken == null || userIdToken == Guid.Empty)
            {
                throw new Exception("Your token is broken.");
            }
            else if (userIdToken != userId)
            {
                throw new Exception("You can't add vocabulary to this book if you don't own it.");
            }
            else
            {
                var vocabulary = _mapper.Map<Vocabulary>(vocabularyCreateDto);
                vocabulary.Id = new Guid();
                await _unitOfWork.Vocabularies.AddAsync(vocabulary);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task DeleteVocabularyAsync(Guid id)
        {

            Guid? userIdToken = _currentUserService.UserId;

            var userId = _unitOfWork.Vocabularies.GetUserIdByVocabularyId(id);

            if (userIdToken == null || userIdToken == Guid.Empty)
            {
                throw new Exception("Your token is broken.");
            }
            else if (userIdToken != userId)
            {
                throw new Exception("You can't delete vocabulary in this book if you don't own it.");
            }

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

        public async Task<PageResultDto<VocabularyDto>> GetAllVocabularyAsync(int page, int pageSize)
        {
            var query = _unitOfWork.Vocabularies.GetAllForPaging();

            var totalItem = query.Count();

            var vocabularies = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PageResultDto<VocabularyDto>
            {
                Items = _mapper.Map<List<VocabularyDto>>(vocabularies),
                TotalItems = totalItem,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<PageResultDto<VocabularyInBookDto>> GetVocabularyByBookIdAsync(Guid bookId, int page, int pageSize)
        {
            Guid? userIdToken = _currentUserService.UserId;

            var userId = _unitOfWork.Vocabularies.GetUserIdByBookId(bookId);

            if (userIdToken == null || userIdToken == Guid.Empty)
            {
                throw new Exception("Your token is broken.");
            }
            else if (userIdToken != userId)
            {
                throw new Exception("You can't see vocabularies in this book if you don't own it.");
            }

            var query = _unitOfWork.Vocabularies.GetAllVocabulariesByBookIdAsync(bookId);

            var totalItem = query.Count();

            var vocabularies = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PageResultDto<VocabularyInBookDto>
            {
                Items = _mapper.Map<List<VocabularyInBookDto>>(vocabularies),
                TotalItems = totalItem,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<VocabularyDto> GetVocabularyByIdAsync(Guid id)
        {
            Guid? userIdToken = _currentUserService.UserId;

            var userId = _unitOfWork.Vocabularies.GetUserIdByVocabularyId(id);

            if (userIdToken == null || userIdToken == Guid.Empty)
            {
                throw new Exception("Your token is broken.");
            }
            else if (userIdToken != userId)
            {
                throw new Exception("You can't see vocabulary if you don't own it.");
            }

            var vocabulary = await _unitOfWork.Vocabularies.GetByIdAsync(id);
            if (vocabulary == null)
            {
                throw new KeyNotFoundException("Vocabulary not found");
            }
            return _mapper.Map<VocabularyDto>(vocabulary);
        }

        public async Task UpdateVocabularyAsync(VocabularyUpdateDto vocabularyUpdateDto, Guid id)
        {
            Guid? userIdToken = _currentUserService.UserId;

            var userId = _unitOfWork.Vocabularies.GetUserIdByVocabularyId(id);

            if (userIdToken == null || userIdToken == Guid.Empty)
            {
                throw new Exception("Your token is broken.");
            }
            else if (userIdToken != userId)
            {
                throw new Exception("You can't update vocabulary in this book if you don't own it.");
            }

            var vocaUpdate = await _unitOfWork.Vocabularies.GetByIdAsync(id);
            if (vocaUpdate == null)
            {
                throw new Exception("Vocabulary not found");
            }
            else
            {
                var vocabulary = _mapper.Map<Vocabulary>(vocabularyUpdateDto);
                vocabulary.Id = id;
                vocabulary.BookId = vocaUpdate.BookId;
                await _unitOfWork.Vocabularies.Update(vocabulary);
                var result = await _unitOfWork.SaveChangesAsync();
                return;
            }
        }
    }
}
