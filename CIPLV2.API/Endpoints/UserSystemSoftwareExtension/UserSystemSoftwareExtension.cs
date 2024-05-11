using CIPLV2.DAL.DataService;
using CIPLV2.Models.UserSystemHardware;
using CIPLV2.Models.UserSystemSoftware;

namespace CIPLV2.API.Endpoints
{
    public static partial class UserSystemSoftwareExtension
    {
        public static async Task<IResult> UserSystemSoftware(IDataService service, List<UserSystemSoftwareDto> dtos)
        {
            var data = await service.userSystemSoftwareData.AddUserSystemSoftware(dtos);
            return Results.Ok(data);
        }

        public static async Task<IResult> GetUserSystemSoftware(IDataService service)
        {
            var data = await service.userSystemSoftwareData.GetAllSystemSoftware();
            return Results.Ok(data);
        }
        public static async Task<IResult> AddSystemHardware(IDataService service, List<UserSystemHardwareDto> dtos)
        {
            var data = await service.userSystemSoftwareData.AddUserSystemHardware(dtos);
            return Results.Ok(data);

        }
        public static async Task<IResult> GetallHardware(IDataService service)
        {
            var data = await service.userSystemSoftwareData.GetAllSystemHardware();
            return Results.Ok(data);    
        }

    }
}
