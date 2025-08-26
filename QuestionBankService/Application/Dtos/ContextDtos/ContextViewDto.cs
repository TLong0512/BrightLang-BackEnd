using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.ContextDtos
{
    public class ContextViewDto
    {
        public Guid Id { get; set; }
        public string RangeName { get; set; }
        public string Content { get; set; }
        public string Explain { get; set; }
        public bool IsBelongTest { get; set; }
    }
}
