
using AutoMapper;
using CIPLV2.DAL.Unitofworks;
using CIPLV2.Models.Admin;
using CIPLV2.Models.PersonDetail;

namespace CIPLV2.DAL.DataService
{
	public class PersonDetailsData : IPersonDetailsData
	{
		readonly IUnitOfWorks _uow;
		readonly IMapper _mapper;
		public PersonDetailsData(IUnitOfWorks uow, IMapper mapper)
		{
			_uow = uow;
			_mapper = mapper;
		}
		public async Task<Response> GetPersonDetails()
		{
			Response response = new Response();
			try
			{
				var data = await _uow.personDetails.GetAllAsync();
				if (data != null && data.Count() > 0)
				{
					var dbdata = _mapper.Map<List<PersonDetailsDto>>(data);
					response.Status = "Success";
					response.Message = "Data Fetch Successfully";
					response.Data = dbdata;
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
		public async Task<Response> AddPersonDetails(List<PersonDetailsDto> data)
		{
			Response response = new Response();
			try
			{
				var mappeddata = _mapper.Map<List<PersonDetails>>(data);
				var newdata = mappeddata.Where(x => x.Id == 0).ToList();
				var existingdata = mappeddata.Where(x => x.Id > 0).ToList();
				if (newdata != null && newdata.Count() > 0)
				{
					_uow.personDetails.AddRange(newdata);
				}
				if (existingdata != null && existingdata.Count() > 0)
				{
					_uow.personDetails.UpdateRange(existingdata);
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
        public async Task<Response> SearchPersonDetails(string value)
        {
            Response response = new Response();
            try
            {
                //var data = await _uow.personDetails.GetAllAsync(x => value.Contains(x.entity_type));
                var data = await _uow.personDetails.GetAllAsync(x => x.EmployeeNumber.Contains(value)||x.Name.Contains(value)||x.FirstName.Contains(value)||x.Upn.Contains(value)||x.Email.Contains(value));
                if (data != null && data.Count() > 0)
                {
                    var dbdata = _mapper.Map<List<PersonDetailsDto>>(data);
                    response.Status = "Success";
                    response.Message = "Data Fetch Successfully";
                    response.Data = dbdata;
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
