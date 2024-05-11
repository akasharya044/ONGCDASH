using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.Models.Admin
{
    public class Test
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }

    }
    public class TestDTO
    {
         
        public int TestId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }

    }
}
