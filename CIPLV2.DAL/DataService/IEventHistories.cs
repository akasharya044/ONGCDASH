using CIPLV2.Models.Admin;
using CIPLV2.Models.EventHistories;

namespace CIPLV2.DAL.DataService
{
    public interface IEventHistories
    {
        Task<Response> GetAllEventHistory();
		Task<Response> AddEventHistory(EventHistoryDto data);
		Task<Response> GetAllEventName(string eventname);

	}
}
