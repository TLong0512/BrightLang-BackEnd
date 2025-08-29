using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.QuestionDtos
{
    public class TestQuestionAddDto
    {
        public Guid QuestionId { get; set; }
        public Guid TestId { get; set; }
    }
}
