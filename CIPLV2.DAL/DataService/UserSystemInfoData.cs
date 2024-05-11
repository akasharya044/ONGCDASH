using AutoMapper;
using CIPLV2.DAL.Unitofworks;
using CIPLV2.Models.Admin;
using CIPLV2.Models.UserSystemHardware;
using CIPLV2.Models.UserSystemSoftware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.DAL.DataService
{
    public class UserSystemSoftwareData : IUserSystemSoftwareData
    {
        readonly IUnitOfWorks _uow;
        readonly IMapper _mapper;
        public UserSystemSoftwareData(IMapper mapper,IUnitOfWorks uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

		public async Task<Response> AddUserSystemSoftware(List<UserSystemSoftwareDto> data)
		{
			Response response = new Response();
			try
			{
				var mappeddata = _mapper.Map<List<UserSystemSoftware>>(data);
				var dbdata = await _uow.UserSystemSoftware.GetAllAsync(x => x.SystemId.ToLower() == data.FirstOrDefault().SystemId.ToLower());
				if (dbdata != null && dbdata.Count() > 0)
				{
					var itemsNotInDb = dbdata.Where(dbItem => !mappeddata.Any(mappedItem => mappedItem.Name == dbItem.Name));

					var itemsInDbNotInMappedData = dbdata.Where(dbItem => !dbdata.Any(mappedItem => mappedItem.Name == dbItem.Name));

					foreach (var item in itemsNotInDb)
					{
						//item.IsActive = false;
						//item.UnInstalled = DateTime.Now;
						_uow.UserSystemSoftware.Update(item);
					}

					foreach (var item in itemsInDbNotInMappedData)
					{
						//item.InstalledOn = DateTime.Now;
						_uow.UserSystemSoftware.Add(item);
					}

				}
				else
				{
					_uow.UserSystemSoftware.AddRange(mappeddata);
				}
				await _uow.SaveAsync();
				var retdata = _mapper.Map<List<UserSystemSoftwareDto>>(mappeddata);
				response.Status = "Success";
				response.Message = "Data Saved";
				response.Data = retdata;
			}
			catch (Exception ex)
			{
				response.Status = "Failed";
				var errormessage = await _uow.AddException(ex);
				response.Message = errormessage;

			}
			return response;

		}

		public async Task<Response> GetAllSystemSoftware()
        {
            Response response = new Response();
            try
            {
                var data = await _uow.UserSystemSoftware.GetAllAsync();
				var mapdata = _mapper.Map<List<UserSystemSoftwareDto>>(data);

                if (mapdata != null && mapdata.Count() > 0)
                {
                    response.Status = "Success";
                    response.Message = "Data Fetch Successfully";
                    response.Data = mapdata;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "No Record Found!!";
                    response.Data = mapdata;
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
		public async Task<Response> AddUserSystemHardware(List<UserSystemHardwareDto> data)
		{
			Response response = new Response();
			try
			{
				List<UserSystemHardware> hardinfo = new List<UserSystemHardware>();
				var mappeddata = _mapper.Map<List<UserSystemHardware>>(data);

				var dbdata = await _uow.UserSystemHardware.GetAllAsync(x => x.SystemId.ToLower() == data.FirstOrDefault().SystemId.ToLower());
				if (dbdata != null && dbdata.Count() > 0)
				{


					var itemsNotInDb = dbdata.Where(dbItem => !mappeddata.Any(mappedItem => mappedItem.Name == dbItem.Name));

					var itemsInDbNotInMappedData = dbdata.Where(dbItem => !dbdata.Any(mappedItem => mappedItem.Name == dbItem.Name));

					foreach (var item in itemsNotInDb)
					{
						_uow.UserSystemHardware.Add(item);
					}

					foreach (var item in itemsInDbNotInMappedData)
					{
						//item.IsActive = false;
						//item.UnInstalled = DateTime.Now;
						_uow.UserSystemHardware.Update(item);
					}

				}

				else
				{
					_uow.UserSystemHardware.AddRange(mappeddata);
				}

				await _uow.SaveAsync();
				var retdata = _mapper.Map<List<UserSystemHardwareDto>>(mappeddata);
				response.Status = "Success";
				response.Message = "Data Saved";
				response.Data = retdata;
			}
			catch (Exception ex)
			{
				response.Status = "Failed";
				var errormessage = await _uow.AddException(ex);
				response.Message = errormessage;

			}
			return response;

		}
		public async Task<Response> GetAllSystemHardware()
        {
            Response response = new Response();
            try
            {
                var data = await _uow.UserSystemHardware.GetAllAsync();
				var mapdata = _mapper.Map<List<UserSystemHardwareDto>>(data);

				if (mapdata != null && mapdata.Count() > 0)
                {
                    response.Status = "Success";
                    response.Message = "Data Fetch Successfully";
                    response.Data = mapdata;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "No Record Found!!";
                    response.Data = mapdata;
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
