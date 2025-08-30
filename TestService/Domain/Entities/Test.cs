using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Test : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int Score { get; set; } = 0;
        public List<TestQuestion> TestQuestions { get; set; }   
        public List<TestAnswer> TestAnswers { get; set; }
    }
}
