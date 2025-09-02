using Application.Dtos.AnswerDtos.AnswerAddDtos;
using Application.Dtos.AnswerDtos.AnswerUpdateDtos;
using Application.Dtos.AnswerDtos.AnswerViewDtos;
using Application.Dtos.ContextDtos;
using Application.Dtos.ContextDtos.ContestAddDto;
using Application.Dtos.ContextDtos.ContextUpdateDtos;
using Application.Dtos.ExamTypeDtos;
using Application.Dtos.LevelDtos;
using Application.Dtos.QuestionDtos.QuestionAddDtos;
using Application.Dtos.QuestionDtos.QuestionUpdateDtos;
using Application.Dtos.QuestionDtos.QuestionViewDtos;
using Application.Dtos.RangeDtos;
using Application.Dtos.SkillDtos;
using Application.Dtos.SkillLevelDto;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Range = Domain.Entities.Range;

namespace Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ExamTypeViewDto, ExamType>().ReverseMap();
            CreateMap<ExamTypeAddDto, ExamType>().ReverseMap();
            CreateMap<ExamTypeUpdateDto, ExamType>().ReverseMap();

            CreateMap<Level, LevelViewDto>()
                .ForMember(x => x.ExamTypeName, y => y.MapFrom(z => z.ExamType.Name)).ReverseMap();
            CreateMap<Level, LevelAddDto>().ReverseMap();
            CreateMap<Level, LevelUpdateDto>().ReverseMap();

            CreateMap<SkillLevel, SkillLevelViewDto>()
                .ForMember(x => x.SkillName, y => y.MapFrom(z => z.Skill.SkillName))
                .ForMember(x => x.LevelName, y => y.MapFrom(z => z.Level.Name))
                .ReverseMap();
            CreateMap<SkillLevelAddDto, SkillLevel>().ReverseMap();
            CreateMap<SkillLevelUpdateDto, SkillLevel>().ReverseMap();

            CreateMap<Skill, SkillViewDto>().ReverseMap();

            CreateMap<Range, RangeViewDto>().ReverseMap();
            CreateMap<Range, RangeAddDto>().ReverseMap();
            CreateMap<Range, RangeUpdateDto>().ReverseMap();
            CreateMap<Range, RangeWithQuestionNumberDto>().ReverseMap();

            CreateMap<Context, ContextAddDto>().ReverseMap();
            CreateMap<Context, ContextViewDto>().ReverseMap();
            CreateMap<Context, ContextUpdateDto>().ReverseMap();

            CreateMap<Question, QuestionAddDto>().ReverseMap();
            CreateMap<Question, QuestionViewDto>()
                .ReverseMap();
            CreateMap<Question, QuestionUpdateDto>().ReverseMap();


            CreateMap<Answer, AnswerAddDto>().ReverseMap();
            CreateMap<Answer, AnswerUpdateDto>().ReverseMap();
            CreateMap<Answer, AnswerViewDto>().ReverseMap();
            CreateMap<AnswerSummaryDto, Answer>().ReverseMap();
        }
    }
}
