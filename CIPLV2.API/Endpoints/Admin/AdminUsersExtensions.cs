using CIPLV2.DAL.DataService;
using CIPLV2.Models.Admin;

namespace CIPLV2.API.Endpoints
{
	public static partial class AdminUsersExtensions
	{
		public static async Task<IResult> Login(AdminUsersDto data, IDataService dataService)
		{
			var response = await dataService.adminData.Login(data);
			return Results.Ok(response);
		}
		public static async Task<IResult> AddUser(AdminUsersDto data, IDataService dataService)
		{
			var response = await dataService.adminData.UpsertAdminUsers(data);
			return Results.Ok(response);
		}
		public static async Task<IResult> LoginDashboard(AdminUsersDto data, IDataService dataService)
		{
			var response = await dataService.adminData.Logindashboard(data);
			return Results.Ok(response);
		}
	}
}
