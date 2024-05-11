
using AutoMapper;
using CIPLV2.DAL.Processes;
using CIPLV2.DAL.Unitofworks;
using CIPLV2.Models.Admin;
using CIPLV2.Models.DeviceDetail;
using System.ComponentModel;
using System.Xml.Linq;

namespace CIPLV2.DAL.DataService
{
    public class DeviceDetailsData : IDeviceDetailsData
    {
        readonly IUnitOfWorks _uow;
        readonly IMapper _mapper;
        readonly BackgroundProcesses _worker;
        readonly DeviceLogs _deviceLogs;
        public DeviceDetailsData(IUnitOfWorks uow, IMapper mapper, DeviceLogs deviceLogs)
        {
            _uow = uow;
            _mapper = mapper;
            _deviceLogs = deviceLogs;
        }
        public async Task<Response> GetDeviceDetails()
        {
            Response response = new Response();
            try
            {
                var data = await _uow.deviceDetails.GetAllAsync();
				var ticdata = await _uow.ticketrecord.GetAllAsync();

				if (data != null && data.Count() > 0)
                {
                    var dbdata = _mapper.Map<List<DeviceDetailsDto>>(data);
					foreach (var item in dbdata)
					{
                        if (ticdata != null)
                        {
                            var correspondingData = ticdata.FirstOrDefault(d => d.SystemId!.ToLower() == item.properties.DisplayLabel!.ToLower());
                            if (correspondingData != null)
                            {
                                item.properties.Location = correspondingData.Location;
                            }
                        }
					}
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
        public async Task<Response> AddDeviceDetails(List<DeviceDetailsDto> data)
        {
            Response response = new Response();
            try
            {

                var mappeddata = _mapper.Map<List<DeviceDetails>>(data);

                var newdata = mappeddata.Where(x => x.Id == 0).ToList();
                var existingdata = mappeddata.Where(x => x.Id > 0).ToList();
                if (newdata != null && newdata.Count() > 0)
                {
                    _uow.deviceDetails.AddRange(newdata);
                }
                if (existingdata != null && existingdata.Count() > 0)
                {
                    _uow.deviceDetails.UpdateRange(existingdata);
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
		public async Task<Response> RegisterDevice(DeviceDetailsDto data)
		{
			Response response = new Response();
			try
			{

				var mappeddata = _mapper.Map<DeviceDetails>(data);
				var dbdata = _uow.deviceDetails.GetAll(x => x.DisplayLabel.ToLower() == data.properties.DisplayLabel.ToLower()).FirstOrDefault();
				if (dbdata != null)
				{
					dbdata.LastUpdateTime = data.properties.LastUpdateTime;
					dbdata.AgentVersion = data.properties.AgentVersion;
					dbdata.IpAddress = data.properties.IpAddress;
					dbdata.MacAddress = data.properties.MacAddress;
					dbdata.IsDeleted = false;
                    dbdata.SubType = data.properties.SubType;
					_uow.deviceDetails.Update(dbdata);
					await _uow.SaveAsync();
				}
				else
				{

					_uow.deviceDetails.Add(mappeddata);
					await _uow.SaveAsync();
				}
                //LogWriter.LogWrite("after device register");
                var devicestatusdata = _uow.deviceDailyStatus.GetAll(x => x.SystemId.ToLower() == data.properties.DisplayLabel.ToLower()).FirstOrDefault();
                if(devicestatusdata != null)
                {
                    devicestatusdata.LastLoginTime=DateTime.Now;
                    _uow.deviceDailyStatus.Update(devicestatusdata);
					await _uow.SaveAsync();
				}
                else
                {
					//LogWriter.LogWrite("Enter into else condition of devicestatusdata");

					DeviceDailyStatus device = new DeviceDailyStatus();
                    device.SystemId = data.properties.DisplayLabel;
                    device.LastLoginTime = DateTime.Now;
                    _uow.deviceDailyStatus.Add(device);
					await _uow.SaveAsync();

				}
				response.Status = "Success";
				response.Message = "Data Saved";
				response.Data = mappeddata;
                LogWriter.LogWrite("Register Successfully");
			}
			catch (Exception ex)
			{
                LogWriter.LogWrite(ex.ToString());
                response.Status = "Failed";
				var errormessage = await _uow.AddException(ex);
				response.Message = errormessage;

			}
			return response;
		}
		public async Task<Response> AddDeviceRunningLog(DeviceRunningLogDTO data)
        {
            Response response = new Response();
            try
            {
                var dbdata = _mapper.Map<DeviceRunningLog>(data);

                _uow.deviceRunningLog.Add(dbdata);

                await _uow.SaveAsync();
                response.Status = "Success";
                response.Message = "Data Saved";
                response.Data = null;
            }
            catch (Exception ex)
            {
                response.Status = "Failed";
                var errormessage = await _uow.AddException(ex);
                response.Message = errormessage;

            }
            return response;
        }
        public async Task<Response> SearchDeviceDetails(string value)
        {
            Response response = new Response();
            try
            {
                var data = await _uow.deviceDetails.GetAllAsync(x => x.DisplayLabel.Contains(value));
                if (data != null && data.Count() > 0)
                {
                    var dbdata = _mapper.Map<List<DeviceDetailsDto>>(data);
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
        public async Task<Response> GetDeviceStatusPool()
        {
            Response response = new Response();
            try
            {
                
                    response.Status = "Success";
                    response.Message = "Data Sent";
                    response.Data = _deviceLogs.GetDevicePool();
                
            }
            catch (Exception ex)
            {
                response.Status = "Failed";
                var errormessage = await _uow.AddException(ex);
                response.Message = errormessage;
            }
            return response;

        }
        public async Task<Response> TriggerFeedback(string machineid)
        {
            Response response = new Response();
            try
            {
                await _deviceLogs.TriggerFeedback(machineid);
                response.Status = "Success";
                response.Message = "Triggred";
                response.Data = null;

            }
            catch (Exception ex)
            {
                response.Status = "Failed";
                var errormessage = await _uow.AddException(ex);
                response.Message = errormessage;
            }
            return response;

        }

		public async Task<Response> GetDeviceDetail()
		{
			Response response = new Response();
			try
			{
				var data = await _uow.deviceDetails.GetAllAsync(x=>x.IsDeleted==false);
				var systemIds = data.Select(x => x.DisplayLabel.ToLower()).ToList();

				var ticdata = await _uow.ticketrecord.GetAllAsync(x=>systemIds.Contains(x.SystemId.ToLower()));

				if (data != null && data.Count() > 0)
				{
					var dbdata = _mapper.Map<List<DeviceDetailsDto>>(data);
					foreach (var item in dbdata)
					{
						if (ticdata != null)
						{
							var correspondingData = ticdata.FirstOrDefault(d => d.SystemId!.ToLower() == item.properties.DisplayLabel!.ToLower());
							if (correspondingData != null)
							{
								item.properties.Location = correspondingData.Location;
							}
						}
					}
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


		public async Task<Response> GetDeviceDailyStatusDetail()
		{
			Response response = new Response();
			try
			{
				var data = await _uow.deviceDailyStatus.GetAllAsync();

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
