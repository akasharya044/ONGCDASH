using CIPLV2.DAL.DataService;
using CIPLV2.Models.AdditionalInfo;
using CIPLV2.Models.Admin;

namespace CIPLV2.API.Endpoints;

public static partial class AdditionalExtensions
{
	public static async Task<IResult> UpsertOSInformation(AdditionalInformationDTO data, IDataService dataService)
	{
		var response = await dataService.additionalInfo.AddOSInfromation(data);
		return Results.Ok(response);
	}

	public static async Task<IResult> GetOsInformation(IDataService dataService)
	{
		var response = await dataService.additionalInfo.GetOsInformation();
		return Results.Ok(response);

    }


    public static async Task<IResult> AddHardDiskInfo(AdditionalInformationHardDiskDto data , IDataService dataService)
	{
		var response = await dataService.additionalInfo.AddHardDiskInfo(data);
		return Results.Ok(response);
	}

	public static async Task<IResult> DiskInfo (IDataService dataservice)
	{
		var response = await dataservice.additionalInfo.Diskinfo();
		return Results.Ok(response);

    }

	public static async Task<IResult> Addservicesinfo (List<NoServicesDto> data , IDataService dataService)
	{
		var response = await dataService.additionalInfo.Addservicesinfo(data);
		return Results.Ok(response);
	}

	public static async Task<IResult> GetServiceInfo(IDataService dataService)
	{
		var response = await dataService.additionalInfo.Getservicesinfo();
		return Results.Ok(response);
	}

	public static async Task<IResult> AddDeviceData (DeviceDataDto data , IDataService dataService)
	{
		var response = await dataService.additionalInfo.AddDeviceData(data);
		return Results.Ok(response);
	}

	public static async Task<IResult> GetDeviceDataInfo (IDataService dataService)
	{
		var response = await dataService.additionalInfo.GetDeviceDataInfo();
		return Results.Ok(response);
	}

}
