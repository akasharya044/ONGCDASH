

using CIPLV2.Models.AdditionalInfo;
using CIPLV2.Models.Admin;
using CIPLV2.Models.Area;

namespace CIPLV2.DAL.DataService;

public interface IAdditionalInfo
{
	Task<Response> AddOSInfromation(AdditionalInformationDTO data);

	Task<Response> GetOsInformation();

    Task<Response> AddHardDiskInfo(AdditionalInformationHardDiskDto data);

	Task<Response> Diskinfo();
    Task<Response> Addservicesinfo(List<NoServicesDto> data);

	Task<Response> Getservicesinfo();

	Task<Response> AddDeviceData (DeviceDataDto data);

	Task<Response> GetDeviceDataInfo();

}
