namespace CIPLV2.API.Endpoints
{	
		public static partial class CsatSettingExtensions
		{
			public static RouteGroupBuilder MapCsatEndpoint(this RouteGroupBuilder group)
			{
				group.MapPost("", CsatSetting);
				group.MapGet("", GetCsatSetting);
				return group;
			}
		}
	
}
