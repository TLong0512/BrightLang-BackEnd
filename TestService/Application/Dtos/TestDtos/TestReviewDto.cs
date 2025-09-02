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
        public Guid Id { get; set; }
        public string CreatedDate { get; set; }
        public int Duration { get; set; }
        public IEnumerable<QuestionDetailDto> QuestionDetails { get; set; } 
        public IEnumerable<Guid> ChoseAnswerIds { get; set; }
    }
}
