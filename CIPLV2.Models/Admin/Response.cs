using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.Models.Admin
{
    public class Response
    {
        public string Status {  get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public string StatusCode { get; set; }
    }
}
