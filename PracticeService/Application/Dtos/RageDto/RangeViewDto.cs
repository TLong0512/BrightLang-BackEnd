using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.RageDto
{
    public class RangeViewDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int StartQuestionNumber { get; set; }
        public int EndQuestionNumber { get; set; }
    }
}
