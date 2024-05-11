using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.Models.AdditionalInfo
{
    public  class NoServices
    {
        [Key]
        public int Id { get; set; }
        public string? ServiceName { get; set; }
        public string? ServiceDisplayName { get; set; }

        public string? ServiceStatus { get; set; }
        public bool?  startup { get; set; }

    
        public string? SystemId { get; set; }
    }

    public class NoServicesDto
    {
        public string? ServiceName { get; set; }
        public string? ServiceDisplayName { get; set; }

        public string? ServiceStatus { get; set; }
        public bool? startup { get; set; }

        public string? SystemId { get; set; }
    }
}
