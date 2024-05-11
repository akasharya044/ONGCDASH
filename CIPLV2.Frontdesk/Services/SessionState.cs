using CIPLV2.Frontdesk.Components.Dto;

namespace CIPLV2.Frontdesk.Services
{
    public static class SessionState
    {
        public static DeviceDto Device { get; set; }=new DeviceDto { TotalDevices = 5, RunningDevices = 2 };
    }
}
