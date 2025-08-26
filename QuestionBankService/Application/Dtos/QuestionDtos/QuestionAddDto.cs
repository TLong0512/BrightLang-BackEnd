using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.QuestionDtos
{
    public class QuestionAddDto
    {
        public Guid ContextId { get; set; }
        public int QuestionNumber { get; set; }
        public string Content { get; set; }
        public string Explain { get; set; }
    }
}
