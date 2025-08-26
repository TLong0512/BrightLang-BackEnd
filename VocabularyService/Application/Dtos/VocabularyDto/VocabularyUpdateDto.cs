using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.VocabularyDto
{
    public class VocabularyUpdateDto
    {
        public string Front { get; set; }
        public string Back { get; set; }
    }
}
