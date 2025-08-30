using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TestQuestion : BaseEntity
    {
        public Guid QuestionId { get; set; }
        public Guid TestId { get; set; }
        public Test Test { get; set; }
    }
}
