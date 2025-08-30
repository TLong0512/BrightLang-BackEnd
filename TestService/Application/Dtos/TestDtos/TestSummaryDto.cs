using Application.Dtos.QuestionDtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.TestDtos
{
    public class TestSummaryDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Score { get; set; }
    }
}
