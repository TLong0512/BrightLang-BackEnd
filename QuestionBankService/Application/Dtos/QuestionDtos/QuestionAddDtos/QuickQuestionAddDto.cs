using Application.Dtos.AnswerDtos.AnswerAddDtos;
using Application.Dtos.ContextDtos.ContextAddDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.QuestionDtos.QuestionAddDtos
{
    public class QuickQuestionAddDto
    {
        public int QuestionNumber { get; set; }
        public string Content { get; set; }
        public string Explain { get; set; }
        public QuickContextAddDto Context { get; set; }  
        public IEnumerable<QuickAnswerAddDto> AnswerList { get; set; }
    }
}
