namespace CIPLV2.API.Endpoints
{
	public static partial class AdminUsersExtensions
	{
		public static RouteGroupBuilder MapAdminEndpoint(this RouteGroupBuilder group)
		{
			group.MapPost("/login", Login);
			group.MapPost("/user", AddUser);
			group.MapPost("/dash/login", LoginDashboard);
			return group;
		}
	}
}
