using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.AnswerDtos
{
    public class SubmitTestRequestDto
    {
        public List<Guid> listAnswerIds { get; set; }
        public List<Guid> listTrueAnswerIds { get; set; }
    }
}
