using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.AnswerDtos
{
    public class AnswerUpdateDto
    {
        public Guid QuestionId { get; set; }
        public string Value { get; set; }
        public string Explain { get; set; }
        public bool IsCorrect { get; set; }
    }
}
