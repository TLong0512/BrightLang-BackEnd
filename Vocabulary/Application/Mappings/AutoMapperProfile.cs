using Application.Dtos.BookDto;
using Application.Dtos.VocabularyDto;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<VocabularyUpdateDto, Vocabulary>().ReverseMap();
            CreateMap<VocabularyCreateDto, Vocabulary>().ReverseMap();
            CreateMap<VocabularyDto, Vocabulary>().ReverseMap();
            CreateMap<VocabularyInBookDto, Vocabulary>().ReverseMap();

            CreateMap<BookDto, Book>().ReverseMap();
            CreateMap<BookCreateDto, Book>().ReverseMap();
            CreateMap<BookUpdateDto, Book>().ReverseMap();
            CreateMap<BookOfUserDto, Book>().ReverseMap();
        }
    }
}
