using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.TestDtos
{
    public class TestAddDto
    {
        public Guid UserId { get; set; }
        public int Score { get; set; } = 0;
    }
}
