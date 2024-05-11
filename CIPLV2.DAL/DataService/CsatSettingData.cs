
using AutoMapper;
using CIPLV2.DAL.Unitofworks;
using CIPLV2.Models.Admin;
using CIPLV2.Models.Area;
using CIPLV2.Models.PersonDetail;
using CIPLV2.Models.Tickets;

namespace CIPLV2.DAL.DataService
{
	public class CsatSettingData: ICsatSettingData
	{
		readonly IUnitOfWorks _uow;
		readonly IMapper _mapper;
		public CsatSettingData(IUnitOfWorks uow, IMapper mapper)
		{
			_uow = uow;
			_mapper = mapper;
		}
		public async Task<Response> GetCsatSetting()
		{
			Response response = new Response();
			try
			{
				var data = await _uow.csatsetting.GetAllAsync();
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

		public async Task<Response> UpsertSetting(CsatSetting data)
		{
			Response response = new Response();
			try
			{
				
				var dbdata = await _uow.csatsetting.GetFirstOrDefaultAsync(x=>x.Id == data.Id);
				if (dbdata != null)
				{
					dbdata.FeedbackPopupTime = data.FeedbackPopupTime;
					_uow.csatsetting.Update(dbdata);
					 _uow.Save();
					response.Status = "Success";
					response.Message = "Data Saved";
					response.Data = dbdata;
				}
				else
				{
					_uow.csatsetting.Add(data);
					_uow.Save();
				}
				response.Status = "Success";
				response.Message = "Data Saved";
				response.Data = data;
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
