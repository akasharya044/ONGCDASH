namespace CIPLV2.API.Endpoints
{
    public static partial class AreaExtensions
    {

        public static RouteGroupBuilder MapAreaEndpoint(this RouteGroupBuilder group)
        {
            group.MapPost("", AddArea);
            group.MapGet("", GetArea);
			group.MapGet("/areas", GetAreas);
			group.MapGet("/getarea", GetAreaAll);
			return group;
        }
    }
}
