using Application.Dtos.AnswerDtos;
using Application.Dtos.ContextDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.QuestionDtos
{
    public class QuestionDetailDto
    {
        public QuestionViewDto QuestionInformation { get; set; }
        public ContextViewDto ContextInformation { get; set; }
        public string RangeName { get; set; }
        public string SkillName { get; set; }
        public string LevelName { get; set; }

        public IEnumerable<AnswerViewDto> AnswerDetails { get; set; }
    }
}
