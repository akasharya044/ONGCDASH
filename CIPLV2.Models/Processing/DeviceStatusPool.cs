using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.Models.Processing
{
    public class DeviceStatusPool
    {
        public string DeviceId { get; set; }
        public DateTime LastHeartBeat  { get; set; }
        public bool IsRunning { get; set; } = false;
    }
}
