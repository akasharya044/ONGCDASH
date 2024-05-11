using AutoMapper;
using CIPLV2.DAL.Unitofworks;
using CIPLV2.Models.Admin;
using CIPLV2.Models.EventHistories;

namespace CIPLV2.DAL.DataService
{
    public class EventHistories : IEventHistories
    {
        readonly IUnitOfWorks _uow;
        readonly IMapper _mapper;

        public EventHistories(IUnitOfWorks uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<Response> GetAllEventHistory()
        {
            Response response = new Response();
            try
            {
                var data = await _uow.eventHistory.GetAllAsync();
                if (data != null && data.Count() > 0)
                {
                    response.Status = "Success";
                    response.Message = "Data Fetch Successfully";
                    response.Data = data;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "No Record Found!!";
                    response.Data = data;
                }
            }
            catch (Exception ex)
            {
                response.Status = "Failed";
                var errormessage = await _uow.AddException(ex);
                response.Message = errormessage;
            }
            return response;
        }
		public async Task<Response> AddEventHistory(EventHistoryDto data)
		{
			Response response = new Response();
			try
			{
				var mappeddata = _mapper.Map<EventHistory>(data);

				_uow.eventHistory.Add(mappeddata);


				await _uow.SaveAsync();
				response.Status = "Success";
				response.Message = "Data Saved";
				response.Data = mappeddata;
			}
			catch (Exception ex)
			{
				response.Status = "failed";
				var error = await _uow.AddException(ex);
				response.Message = error;
			}

			return response;
		}
		public async Task<Response> GetAllEventName(string eventname)
		{
			Response response = new Response();
			try
			{
				var data = await _uow.eventHistory.GetAllAsync(x=>x.Event.ToLower().Replace(" ", "") == eventname.ToLower().Replace(" ", ""));
				if (data != null && data.Count() > 0)
				{
					response.Status = "Success";
					response.Message = "Data Fetch Successfully";
					response.Data = data;
				}
				else
				{
					response.Status = "Success";
					response.Message = "No Record Found!!";
					response.Data = data;
				}
			}
			catch (Exception ex)
			{
				response.Status = "Failed";
				var errormessage = await _uow.AddException(ex);
				response.Message = errormessage;
			}
			return response;
		}
	}
}
