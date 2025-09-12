using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.SkillLevelDto
{
    public class SkillLevelViewDto
    {
        public Guid Id { get; set; }
        public Guid SkillId { get; set; }
        public string SkillName { get; set; }   
        public string LevelName { get; set; }
    }
}
