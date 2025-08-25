using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.BookDto
{
    public class BookCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
