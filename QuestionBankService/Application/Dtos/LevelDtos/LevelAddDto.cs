using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.LevelDtos
{
    public class LevelAddDto
    {
        public string Name { get; set; }
        public Guid ExamTypeId { get; set; }  
    }
}
