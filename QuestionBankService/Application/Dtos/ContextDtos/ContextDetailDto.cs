using Application.Dtos.QuestionDtos.QuestionViewDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.ContextDtos
{
    public class ContextDetailDto
    {
        public ContextViewDto ContextViewDto { get; set; }
        public IEnumerable<QuestionViewDto> QuestionViewDtos { get; set; }
    }
}
