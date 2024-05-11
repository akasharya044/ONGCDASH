using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.Models.DeviceDetail
{
    public class DeviceRunningLogDTO
    {
        
        public string SystemId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? ShutDownTime { get; set; }
        
    }
}
