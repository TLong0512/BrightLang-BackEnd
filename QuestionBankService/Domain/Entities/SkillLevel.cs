using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SkillLevel : BaseEntity
    {
        public Guid SkillId { get; set; }   
        public Skill Skill { get; set; }    
        public Guid LevelId { get; set; }   
        public Level Level { get; set; }
        public List<Range> Ranges { get; set; }
    }
}
