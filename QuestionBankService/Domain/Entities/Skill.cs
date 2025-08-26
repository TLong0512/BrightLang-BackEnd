using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Skill : BaseEntity
    {
        public string SkillName { get; set; }
        public List<SkillLevel> SkillLevels { get; set; }
    }
}
