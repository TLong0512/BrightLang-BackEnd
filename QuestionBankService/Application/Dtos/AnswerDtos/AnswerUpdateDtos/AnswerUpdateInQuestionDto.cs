using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.AnswerDtos.AnswerUpdateDtos
{
    public class AnswerUpdateInQuestionDto
    {
        public string Value { get; set; }
        public string Explain { get; set; }
        public bool IsCorrect { get; set; }
    }
}
