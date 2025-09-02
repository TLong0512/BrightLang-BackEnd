using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.ContextDtos.ContextUpdateDtos
{
    public class ContextUpdateDto
    {
        public Guid RangeId { get; set; }
        public string Content { get; set; }
        public string Explain { get; set; }
        public bool IsBelongTest { get; set; }
    }
}
