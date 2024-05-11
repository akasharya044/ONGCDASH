using CIPLV2.DAL.DataService;
using CIPLV2.Models.Tickets;

namespace CIPLV2.API.Endpoints
{
	public static partial class CsatSettingExtensions
	{
		public static async Task<IResult> CsatSetting(CsatSetting data, IDataService dataService)
		{
			var response = await dataService.csatsetting.UpsertSetting(data);
			return Results.Ok(response);
		}
		public static async Task<IResult> GetCsatSetting(IDataService dataService)
		{
			var response = await dataService.csatsetting.GetCsatSetting();
			return Results.Ok(response);
		}
	}
}
