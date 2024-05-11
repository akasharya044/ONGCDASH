namespace CIPLV2.API.Endpoints
{
	public static partial class DeviceDetailsExtensions
	{
		public static RouteGroupBuilder MapDeviceDetailsEndpoint(this RouteGroupBuilder group)
		{

			group.MapPost("", AddDeviceDetails);
			group.MapGet("", DeviceDetailsList);
            group.MapPost("adddevicelog", AddDeviceLog);
			group.MapPost("registerdevice", RegisterDevice);
            group.MapGet("searchdevice/{value}", SearchDeviceDetailsList);
            group.MapGet("getdevicestatus", GetDeviceStatusPool);
            group.MapGet("triggerfeedback", TriggerFeedback);
			group.MapGet("/device", DeviceDetails);
			group.MapGet("/device/dailystatus", DeviceDailyDetails);

			return group;
		}
	}
}
