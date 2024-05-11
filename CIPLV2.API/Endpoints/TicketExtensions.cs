
namespace CIPLV2.API.Endpoints
{
    public static partial class TicketExtensions
    {
        public static RouteGroupBuilder MapTicketEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/getlist",GetList);
            group.MapGet("/filter", GetTicketsbyDate);
            group.MapGet("/getticketlist", GetTicketList);
            group.MapPost("/raiseticket", RaiseTicket);
            group.MapPost("/updateticket", UpdateTicket);
            group.MapPost("/import", UploadTicket);
            group.MapGet("/mfticket", GetMFTicketList);
            group.MapGet("/person", saveperson);
            group.MapGet("/device", savedevice);
            group.MapPost("/starrating", AddStarRating);
			group.MapGet("/saveticket", SaveTicket);
			group.MapGet("/ticData", GetTicketData);

			return group;
        }
    }
}
