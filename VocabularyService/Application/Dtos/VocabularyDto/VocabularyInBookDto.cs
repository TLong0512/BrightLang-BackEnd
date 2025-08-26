using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.VocabularyDto
{
    public class VocabularyInBookDto
    {
        public Guid Id { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }
    }
}
