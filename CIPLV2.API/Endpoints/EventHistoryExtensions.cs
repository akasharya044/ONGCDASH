namespace CIPLV2.API.Endpoints
{
    public  static partial class EventHistoryExtensions
    {
        public static RouteGroupBuilder MapEventHistoryMapEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("",GetAllEventHistory);
			group.MapPost("", AddEventHistory);
			group.MapGet("evenname/{eventname}", GetAllEventByEventName);

			return group;
        }
    }
}
