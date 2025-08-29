using Application.Dtos.AnswerDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.QuestionDtos
{
    public class QuestionSummaryDto
    {
        public int QuestionNumber { get; set; }
        public string Content { get; set; }
        public string ContextContent {get;set;} 
        public string SkillName { get; set; }
        public List<AnswerSummaryDto> AnswerContents { get; set; }
    }
}
