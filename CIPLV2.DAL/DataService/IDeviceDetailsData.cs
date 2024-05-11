
using CIPLV2.Models.Admin;
using CIPLV2.Models.DeviceDetail;
using CIPLV2.Models.PersonDetail;

namespace CIPLV2.DAL.DataService
{
	public interface IDeviceDetailsData
	{
		Task<Response> AddDeviceDetails(List<DeviceDetailsDto> data);
        Task<Response> RegisterDevice(DeviceDetailsDto data);
        Task<Response> GetDeviceDetails();
		Task<Response> AddDeviceRunningLog(DeviceRunningLogDTO data);
        Task<Response> SearchDeviceDetails(string value);
        Task<Response> GetDeviceStatusPool();
        Task<Response> TriggerFeedback(string machineid);
		Task<Response> GetDeviceDetail();
		Task<Response> GetDeviceDailyStatusDetail();
	}
}
