using CIPLV2.DAL.DataService;
using CIPLV2.Models.EventHistories;

namespace CIPLV2.API.Endpoints;

public static  partial class EventHistoryExtensions
{
    public static async Task<IResult> GetAllEventHistory(IDataService dataService)
    {
        var response = await dataService.eventHistory.GetAllEventHistory();
        return Results.Ok(response);
    }
	public static async Task<IResult> AddEventHistory(EventHistoryDto data, IDataService dataService)
	{
		var response = await dataService.eventHistory.AddEventHistory(data);
		return Results.Ok(response);
	}
	public static async Task<IResult> GetAllEventByEventName(IDataService dataService,string eventname)
	{
		var response = await dataService.eventHistory.GetAllEventName(eventname);
		return Results.Ok(response);
	}
}
