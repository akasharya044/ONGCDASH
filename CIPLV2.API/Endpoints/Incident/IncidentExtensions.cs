using CIPLV2.DAL.DataService;
using CIPLV2.Models.Incidents;
using Sieve.Models;

namespace CIPLV2.API.Endpoints;

public static partial class IncidentExtensions
{
    public static async Task<IResult> GetIncident(IDataService dataService, int Id)
    {
        var response = await dataService.incident.GetIncidentById(Id);
        return Results.Ok(response);
    }
    public static async Task<IResult> GetAllIncident(IDataService dataService, [AsParameters] SieveModel model)
    {
        var response = await dataService.incident.GetAllIncident(model);
        return Results.Ok(response);
    }
    public static async Task<IResult> UpsertIncident(IDataService dataService,IncidentDTO incidentDTO)
    {
        var response = await dataService.incident.Upsert(incidentDTO);
        return Results.Ok(response);
    }
   
}
