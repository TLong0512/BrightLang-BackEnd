using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.QuestionDto
{
    public class QuestionViewDto
    {
        public Guid Id { get; set; }
        public int QuestionNumber { get; set; }
        public string Content { get; set; }
        public string Explain { get; set; }
    }
}
