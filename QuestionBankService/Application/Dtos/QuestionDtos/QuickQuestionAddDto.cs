using Application.Dtos.AnswerDtos;
using Application.Dtos.ContextDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.QuestionDtos
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
