using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.RangeDtos
{
    public class RangeUpdateDto
    {
        public string Name { get; set; }
        public int StartQuestionNumber { get; set; }
        public int EndQuestionNumber { get; set; }
        public Guid SkillLevelId { get; set; }
    }
}
