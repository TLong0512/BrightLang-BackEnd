using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.SkillLevelDto
{
    public class SkillLevelAddDto
    {
        public Guid SkillId { get; set; }
        public Guid LevelId { get;set; }
    }
}
