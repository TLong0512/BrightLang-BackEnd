using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TestAnswer : BaseEntity
    {
        public Guid TestId { get; set; }
        public Test Test { get; set; }
        public Guid AnswerId { get; set; }
    }
}
