using CIPLV2.Models.Admin;
using CIPLV2.Models.Tickets;

namespace CIPLV2.DAL.DataService
{
    public interface ITicketData
    {
        Task<Response> GetList();
        Task<Response> GetTicketsBydate(DateTime date1, DateTime date2);
		Task<Response> GetTicketList(string systemid);
        Task<Response> AddTicket(RaiseAgentTicketDTO input);
        Task<Response> UpdateTicket(UpdateTicket updateTicket);
        Task<Response> GetTicketByMFList();
        Task<Response> uploadTicket(List<TicketRecord> tickets);
        Task<Response> SavePersonDetails();
        Task<Response> SaveDeviceDetails();
        Task<Response> SaveStarRating(StarRatingDTO input);
		Task<Response> SaveTicketDetails();
		Task<Response> GetTicketDataList(string ticId);

	}
}
