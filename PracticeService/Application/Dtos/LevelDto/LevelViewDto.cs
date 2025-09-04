using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.LevelDto
{
    public class LevelViewDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ExamTypeName { get; set; }
    }
}
