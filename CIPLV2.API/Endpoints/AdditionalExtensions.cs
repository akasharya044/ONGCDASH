namespace CIPLV2.API.Endpoints;

public static partial class AdditionalExtensions
{
	public static RouteGroupBuilder MapAdditionalEndpoint(this RouteGroupBuilder group)
	{
		group.MapPost("/additionalinfo/Upsert", UpsertOSInformation);
		group.MapPost("/additionalinfo/HardDisk", AddHardDiskInfo);
		group.MapPost("/additionalinfo/NoOfService", Addservicesinfo);
		group.MapPost("/additionalinfo/AddDeviceData", AddDeviceData);
		group.MapGet("/additionalinfo/Osinfo", GetOsInformation);
		group.MapGet("/additionalinfo/harddiskinfo", DiskInfo);
		group.MapGet("additionalinfo/Servicesinfo", GetServiceInfo);
		group.MapGet("additionalinfo/Deviceinfo", GetDeviceDataInfo);
		return group;
	}
}
