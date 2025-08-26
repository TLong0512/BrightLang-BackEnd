using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Range : BaseEntity
    {
        public Guid SkillLevelId { get; set; }
        public SkillLevel SkillLevel { get; set; }
        public string Name { get;set; }
        public int StartQuestionNumber { get; set; }
        public int EndQuestionNumber { get; set; }
        public List<Context> Contexts { get; set; }
    }
}
