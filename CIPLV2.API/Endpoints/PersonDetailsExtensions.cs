namespace CIPLV2.API.Endpoints
{
	public static partial class PersonDetailsExtensions
	{
		public static RouteGroupBuilder MapPersonDetailsEndpoint(this RouteGroupBuilder group)
		{
			
			group.MapPost("", AddPersonDetails);
			group.MapGet("", PersonDetailsList);
            group.MapGet("searchpersondetails/{value}", SearchPersonDetailsList);
            return group;
		}
	}
}
