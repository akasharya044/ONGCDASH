using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.Models.UserSystemHardware
{
    public class UserSystemHardware
    {
        [Key]
        public int Id {  get; set; }
        public string? Name { get; set; }
        public string? value { get; set; }
        public string? Type { get; set; }
        public bool IsLocal {  get; set; }
        public bool IsArray { get; set; }
        public string? Origin { get; set; }
        public string? Qualifires {  get; set; }
        public string? SystemId { get; set; }

	}
}
