
using AutoMapper;
using CIPLV2.DAL.Unitofworks;
using CIPLV2.Models.AdditionalInfo;
using CIPLV2.Models.Admin;

namespace CIPLV2.DAL.DataService;

public class AdditionalInfo : IAdditionalInfo
{
	readonly IUnitOfWorks _uow;
	readonly IMapper _mapper;
	public AdditionalInfo(IUnitOfWorks uow, IMapper mapper)
	{
		_uow = uow;
		_mapper = mapper;
	}
	public async Task<Response> AddOSInfromation(AdditionalInformationDTO data)
	{
		Response response = new Response();
		try
		{
			var DbData = _mapper.Map<AdditionalInformation>(data);
			var checkDbData = _uow.additionalInformation.GetAllNoTracking(x => x.SystemId == DbData.SystemId)!.FirstOrDefault();
			if (checkDbData != null)
			{
				DbData.Id = checkDbData.Id;
				_uow.additionalInformation.Update(DbData);
			}
			else
			{
				_uow.additionalInformation.Add(DbData);
			}
			await _uow.SaveAsync();
			response.Status = "Success";
			response.Message = "Data Saved";
			response.Data = DbData;
		}
		catch (Exception ex)
		{
			response.Status = "Failed";
			var errormessage = await _uow.AddException(ex);
			response.Message = errormessage;

		}
		return response;
	}

	public async Task<Response> GetOsInformation()
	{
		Response response = new Response();
		try
		{

			var data = await _uow.additionalInformation.GetAllAsync();
			var dbData = _mapper.Map<List<AdditionalInformationDTO>>(data);
			if (data != null && data.Count() > 0)
			{
				response.Status = "Success";
				response.Message = "Data Fetched";
				response.Data = data;

			}
			else
			{
				response.Status = "Success";
				response.Message = "No Record Found";
				response.Data = dbData;
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

	public async Task<Response> AddHardDiskInfo(AdditionalInformationHardDiskDto data)
	{
		Response response = new Response();
		try
		{
			var Dbdata = _mapper.Map<AdditionalInformationHardDisk>(data);
			var checkDbdata = _uow.harddiskinfo.GetAllNoTracking(x => x.SystemId == Dbdata.SystemId)!.FirstOrDefault();
			if (checkDbdata != null)
			{
				Dbdata.Id = checkDbdata.Id;
				_uow.harddiskinfo.Update(Dbdata);
			}
			else
			{
				_uow.harddiskinfo.Add(Dbdata);
			}
			await _uow.SaveAsync();
			response.Status = "Success";
			response.Message = "Data Saved";
			response.Data = Dbdata;

		}
		catch (Exception ex)
		{
			response.Status = "Failed";
			var errormessage = await _uow.AddException(ex);
			response.Message = errormessage;

		}
		return response;
	}

	public async Task<Response> Diskinfo()
	{
		Response response = new Response();
		try
		{
			var data = await _uow.harddiskinfo.GetAllAsync();
			var dbdata = _mapper.Map<List<AdditionalInformationHardDiskDto>>(data);
			if (data != null && data.Count() > 0)
			{
				response.Status = "Success";
				response.Message = "Data Fetched";
				response.Data = data;

			}
			else
			{
				response.Status = "Success";
				response.Message = "No Record Found";
				response.Data = dbdata;
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

	public async Task<Response> Addservicesinfo(List<NoServicesDto> data)
	{
		Response response = new Response();
		try
		{
			var dbdata = _mapper.Map<List<NoServices>>(data);
			var checkdbdata = _uow.noservice.GetAllNoTracking(x => x.SystemId!.ToLower() == dbdata.Select(x => x.SystemId!.ToLower()).FirstOrDefault())!.ToList();
			if (checkdbdata != null && checkdbdata.Count > 0)
			{
				//dbdata.ForEach(x =>
				//{
				//	if (x.ServiceName.ToLower() == checkdbdata.Where(y => y.ServiceName.ToLower() == x.ServiceName).FirstOrDefault().ServiceName.ToLower())
				//	{
				//		x.Id = checkdbdata.Where(y => y.ServiceName.ToLower() == x.ServiceName).FirstOrDefault().Id;

				//	}
				//});
				foreach (var x in dbdata)
				{
					var matchingService = checkdbdata.FirstOrDefault(y => y.ServiceName!.ToLower() == x.ServiceName!.ToLower());

					if (matchingService != null)
					{
						x.Id = matchingService.Id;
					}
				}
				_uow.noservice.UpdateRange(dbdata);
			}
			else
			{
				_uow.noservice.AddRange(dbdata);
			}
			await _uow.SaveAsync();
			response.Status = "Success";
			response.Message = "Data Saved";
			response.Data = dbdata;

		}
		catch (Exception ex)
		{

			response.Status = "Failed";
			var errormessage = await _uow.AddException(ex);
			response.Message = errormessage;

		}

		return response;
	}

	public async Task<Response> Getservicesinfo()
	{
		Response response = new Response();
		try
		{
			var data = await _uow.noservice.GetAllAsync();
			var dbdata = _mapper.Map<List<NoServicesDto>>(data);
			if (data != null && data.Count() > 0)
			{
				response.Status = "Success";
				response.Message = "Data Fetched SuccessFully";
				response.Data = data;
			}
			else
			{
				response.Status = "Success";
				response.Message = "No Record Found";
				response.Data = dbdata;
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

	public async Task<Response> AddDeviceData(DeviceDataDto data)
	{
		Response response = new Response();
		try
		{

			var dbdata = _mapper.Map<DeviceData>(data);
			var checkdbdata = _uow.deviceData.GetAllNoTracking(x => x.SystemId == dbdata.SystemId)!.FirstOrDefault();
			if (checkdbdata != null)
			{
				dbdata.Id = checkdbdata.Id;
				_uow.deviceData.Update(dbdata);
			}
			else
			{
				_uow.deviceData.Add(dbdata);
			}
			await _uow.SaveAsync();
			response.Status = "Sucess";
			response.Message = "Data Saved";
			response.Data = dbdata;
		}
		catch (Exception ex)
		{
			response.Status = "Failed";
			var errormessage = await _uow.AddException(ex);
			response.Message = errormessage;

		}
		return response;
	}


	public async Task<Response> GetDeviceDataInfo()
	{
		Response response = new Response();
		try
		{
			var data = await _uow.deviceData.GetAllAsync();
			var dbdata = _mapper.Map<List<DeviceDataDto>>(data);
			if (data != null && data.Count() > 0)
			{
				response.Status = "Success";
				response.Message = "Data Fetched SuccessFully";
				response.Data = data;
			}
			else
			{
				response.Status = "Success";
				response.Message = "No Record Found";
				response.Data = dbdata;
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
