using AutoMapper;
using CIPLV2.DAL.Unitofworks;
using CIPLV2.Models.Admin;
using CIPLV2.Models.Category;

namespace CIPLV2.DAL.DataService
{
	public class CategoriesData:ICategoriesData
	{
		readonly IUnitOfWorks _uow;
		readonly IMapper _mapper;
		public CategoriesData(IUnitOfWorks uow, IMapper mapper)
		{
			_uow = uow;
			_mapper = mapper;
		}
		public async Task<Response> GetCategories()
		{
			Response response = new Response();
			try
			{
				var data = await _uow.categories.GetAllAsync();
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
		public async Task<Response> AddCategories(List<CategoriesDto> data)
		{
			Response response = new Response();
			try
			{
				var mappeddata = _mapper.Map<List<Categories>>(data);
				var newdata = mappeddata.Where(x=>x.Id== 0).ToList();
				var existingdata = mappeddata.Where(x=>x.Id > 0).ToList();
				if(newdata != null && newdata.Count() > 0)
				{
					_uow.categories.AddRange(newdata);
				}
				if (existingdata != null && existingdata.Count() > 0)
				{
					_uow.categories.UpdateRange(existingdata);
				}
				await _uow.SaveAsync();
				response.Status = "Success";
				response.Message = "Data Saved";
				response.Data = mappeddata;
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
