namespace CIPLV2.API.Endpoints
{
    public static partial class IncidentExtensions
    {
        public static RouteGroupBuilder MapIncidentEndpoint(this RouteGroupBuilder group)
        {

            group.MapGet("{Id}", GetIncident);
            group.MapGet("", GetAllIncident);
            group.MapPost("/Upsert", UpsertIncident);
            return group;
        }
    }
}
