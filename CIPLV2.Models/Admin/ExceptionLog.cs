using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.Models.Admin
{
    
        public class ExceptionLog
        {
            [Key]
            public int PKExceptionId { get; set; }
            public string ExceptionSource { get; set; }
            public string ExceptionURL { get; set; }
            public string ActionMethod { get; set; }
            public string RequestData { get; set; }
            public string ExceptionMessage { get; set; }
            public DateTime Logdate { get; set; }
        }
    
}
