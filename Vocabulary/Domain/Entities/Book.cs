using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Book : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }

        public ICollection<Vocabulary>? Vocabularies { get; set; }
    }
}
