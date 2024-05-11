using CIPLV2.DAL.DataService;
using CIPLV2.DAL.Processes;
using CIPLV2.Models.Tickets;

namespace CIPLV2.API.Endpoints
{
	public static partial class TicketExtensions
	{
		public static async Task<IResult> GetList(IDataService dataService)
		{
			var response = await dataService.ticketData.GetList();
			return Results.Ok(response);
		}
		public static async Task<IResult> GetTicketsbyDate(IDataService dataService, DateTime date1, DateTime date2)
		{
			var response = await dataService.ticketData.GetTicketsBydate(date1, date2);
			return Results.Ok(response);
		}
		public static async Task<IResult> GetTicketList(IDataService dataService, string systemid)
		{
			var response = await dataService.ticketData.GetTicketList(systemid);
			return Results.Ok(response);
		}
		public static async Task<IResult> RaiseTicket(RaiseAgentTicketDTO input, IDataService dataService)
		{
			var response = await dataService.ticketData.AddTicket(input);
			return Results.Ok(response);
		}
		public static async Task<IResult> UpdateTicket(UpdateTicket updateTicket, IDataService dataService)
		{
			var response = await dataService.ticketData.UpdateTicket(updateTicket);
			return Results.Ok(response);
		}
		public static async Task<IResult> GetMFTicketList(IDataService dataService)
		{
			var response = await dataService.ticketData.GetTicketByMFList();
			return Results.Ok(response);
		}
		public static async Task<IResult> UploadTicket(List<TicketRecord> tickets, IDataService dataService)
		{
			var response = await dataService.ticketData.uploadTicket(tickets);
			return Results.Ok(response);
		}
		public static async Task<IResult> saveperson(IDataService dataService)
		{
			var response = await dataService.ticketData.SavePersonDetails();
			return Results.Ok(response);
		}
		public static async Task<IResult> savedevice(IDataService dataService)
		{
			var response = await dataService.ticketData.SaveDeviceDetails();
			return Results.Ok(response);
		}
		public static async Task<IResult> AddStarRating(StarRatingDTO obj, IDataService dataService)
		{
			var response = await dataService.ticketData.SaveStarRating(obj);
			return Results.Ok(response);
		}
		public static async Task<IResult> SaveTicket(IDataService dataService)
		{
			var response = await dataService.ticketData.SaveTicketDetails();
			return Results.Ok(response);
		}
		public static async Task<IResult> GetTicketData(IDataService dataService, string ticId)
		{
			var response = await dataService.ticketData.GetTicketDataList(ticId);
			return Results.Ok(response);
		}
	}
}
