using Application.Dtos.QuestionDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.TestDtos
{
    public class TestReviewDto
    {
        public Guid TestId { get; set; }
        public List<QuestionDetailDto> QuestionDetails { get; set; } 
    }
}
