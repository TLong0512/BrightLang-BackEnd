using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Context : BaseEntity
    {
        public Guid RangeId { get; set; }   
        public Range Range { get; set; }
        public string Content { get; set; } 
        public string Explain { get; set; }
        public bool IsBelongTest { get; set; }
        public List<Question> Questions { get; set; }
    }
}
