using Application.Dtos.AnswerDtos.AnswerUpdateDtos;
using Application.Dtos.ContextDtos.ContextUpdateDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.QuestionDtos.QuestionUpdateDtos
{
    public class QuestionUpdateDto
    {
        public int QuestionNumber { get; set; }
        public string Content { get; set; }
        public string Explain { get; set; }
        public ContextUpdateInQuestionDto ContextUpdate { get; set; }
        public List<AnswerUpdateInQuestionDto> ListAnswers { get; set; }
    }
}
