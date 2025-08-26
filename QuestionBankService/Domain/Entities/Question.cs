using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Question : BaseEntity
    {
        public Guid ContextId { get; set; } 
        public Context Context { get; set; }
        public int QuestionNumber { get; set; }   
        public string Content { get; set; }
        public string Explain { get; set; } 
        public List<Answer> Answers { get; set; }
    }
}
