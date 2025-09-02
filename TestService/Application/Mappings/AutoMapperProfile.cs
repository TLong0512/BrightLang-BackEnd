using Application.Dtos.AnswerDtos;
using Application.Dtos.QuestionDtos;
using Application.Dtos.TestDtos;
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
            CreateMap<TestAddDto, Test>().ReverseMap();

            CreateMap<TestQuestionAddDto, TestQuestion>().ReverseMap();
            CreateMap<Test, TestSummaryDto>()
                .ForMember(dest => dest.CreatedDate,
                           opt => opt.MapFrom(src => src.CreatedDate.HasValue
                               ? src.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")
                               : ""))
                .ReverseMap();
            CreateMap<TestAnswer, TestAnswerDto>().ReverseMap();
            CreateMap<Test, TestReviewDto>()
                .ForMember(dest => dest.CreatedDate, 
                           opt => opt.MapFrom(src => src.CreatedDate.HasValue
                           ? src.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm"): ""))
                .ReverseMap();
        }
    }

}
