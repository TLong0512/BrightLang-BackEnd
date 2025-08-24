using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Vocabulary : BaseEntity
    {
        public string Front { get; set; }
        public string Back { get; set; }

        public Guid BookId { get; set; }
        public Book? Book { get; set; }
    }
}
