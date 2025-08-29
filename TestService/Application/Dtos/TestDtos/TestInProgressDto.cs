using Application.Dtos.QuestionDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.TestDtos
{
    public class TestInProgressDto
    {
        public Guid TestId { get; set; }
        public List<QuestionSummaryDto> ListQuestion { get; set; }
    }
}
