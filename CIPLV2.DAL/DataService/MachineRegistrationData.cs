using AutoMapper;
using CIPLV2.DAL.Unitofworks;
using CIPLV2.Models.Admin;
using CIPLV2.Models.Registration;

namespace CIPLV2.DAL.DataService
{
	public class MachineRegistrationData : IMachineRegistrationData
	{
		readonly IUnitOfWorks _uow;
		readonly IMapper _mapper;
		public MachineRegistrationData(IUnitOfWorks uow, IMapper mapper)
		{
			_uow = uow;
			_mapper = mapper;
		}
		public async Task<Response> AddMachineRegistration(MachineRegistrationDto data)
		{
			Response response = new Response();
			try
			{
				var mappeddata = _mapper.Map<MachineRegistration>(data);
				var dbdata = await _uow.machineRegistration.GetAllAsync(x=>x.systemid==mappeddata.systemid);
				if(dbdata != null && dbdata.Count()>0)
				{
					_uow.machineRegistration.Update(mappeddata);
					_uow.Save();
				}
				else
				{
					await _uow.machineRegistration.AddAsync(mappeddata);
					await _uow.SaveAsync();
				}
				
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
        public async Task<Response> GetMachinesData()
        {
            Response response = new Response();
            try
            {
                var data = await _uow.machineRegistration.GetAllAsync();
                if (data != null && data.Count() > 0)
                {
                    response.Status = "Success";
                    response.Message = "Data Fetch Successfully";
					var mappeddata = _mapper.Map<List<MachineRegistrationDto>>(data);
                    response.Data = mappeddata;
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
