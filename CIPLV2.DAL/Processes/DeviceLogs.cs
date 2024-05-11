using CIPLV2.DAL.Unitofworks;
using CIPLV2.Models.DeviceDetail;
using CIPLV2.Models.Processing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CIPLV2.Models.Tickets;
using System.Net.Http.Json;

namespace CIPLV2.DAL.Processes
{
	public class DeviceLogs
	{
		private List<DeviceStatusPool> _devices = new List<DeviceStatusPool>();
		private readonly IServiceScopeFactory _serviceScopeFactory;
		private readonly IBus _buscontrol;
		int lastid = 0;
		DateTime lastprocessdate = DateTime.Now;
		DateTime lastGetTickets = DateTime.Now.AddDays(-1);
		bool processing = false;
		int totaldevices = 0;
		int runningdevices = 0;
		public DeviceLogs(IServiceScopeFactory serviceScopeFactory, IBus buscontrol)
		{
			_serviceScopeFactory = serviceScopeFactory;

			SetupCleanUpTask();
			// CreateHBChannel();
			_buscontrol = buscontrol;


		}





		private void OnMessageRecived(string message)
		{
			try
			{
				UpdateDevice(message);

			}
			catch (Exception ex)
			{
				//LogWriter.LogWrite("Error in onmessagerecieved" + ex.Message);

			}
		}
		public async Task CheckDeviceAddition()
		{
			try
			{
				//LogWriter.LogWrite("Start CheckDeviceAddition");
				using (var scope = _serviceScopeFactory.CreateScope())
				{
					var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWorks>();
					var lastrecord = uow.deviceDetails.GetAllNoTracking(null, x => x.OrderByDescending(x => x.Id)).FirstOrDefault();
					LogWriter.LogWrite("after lastrecord" + lastrecord);

					if (lastrecord != null)
					{
						//if (lastrecord.Id > lastid)
						//{
						//	//_devices = new List<DeviceStatusPool>();

						foreach (var device in uow.deviceDetails.GetSelectedNoTracking(x => x.DisplayLabel, x => (x.SubType == "Laptop" || x.SubType == "Desktop") && x.IsDeleted == false).Distinct())
						{
							//LogWriter.LogWrite("Start foreach loop of uow.deviceDetails.GetSelectedNoTracking");
							//LogWriter.LogWrite("device name " + device);

							if (!_devices.Where(d => d.DeviceId == device).Any())
							{
								//LogWriter.LogWrite("inside if (!_devices.Where(d => d.DeviceId == device).Any()) condition\n");
								_devices.Add(new DeviceStatusPool { DeviceId = device, IsRunning = false, LastHeartBeat = DateTime.Now.AddDays(-1) });
								//LogWriter.LogWrite("devices " + _devices);
							}
						}
						totaldevices = _devices.Count;
						lastid = lastrecord.Id;
						//LogWriter.LogWrite("devices " + _devices.Count);

						//}

					}
					runningdevices = _devices.Where(x => x.IsRunning).Count();
					//LogWriter.LogWrite("runningdevices " + runningdevices);

				}
			}
			catch (Exception ex)
			{
				LogWriter.LogWrite($"Exception in CheckDeviceAddition  {ex.Message}");

			}
		}
		public List<DeviceStatusPool> GetDevices()
		{
			return _devices;
		}
		public void Init()
		{

			_devices = new List<DeviceStatusPool>();
		}
		public void AddDevice(DeviceStatusPool device)
		{
			_devices.Add(device);
		}

		public void UpdateDevice(string machineid)
		{
			//LogWriter.LogWrite("start Update Device");
			//LogWriter.LogWrite("Devices" + _devices);
			try
			{
				foreach (var item in _devices.Where(x => x.DeviceId.ToLower().Equals(machineid.ToLower())))
				{
					item.IsRunning = true;
					item.LastHeartBeat = DateTime.Now;
				}
				runningdevices = _devices.Where(x => x.IsRunning).Count();

				//LogWriter.LogWrite("runningdevices in update device" + runningdevices);
				//LogWriter.LogWrite("End Update Device");
			}
			catch (Exception ex)
			{
				LogWriter.LogWrite("Exception in Update Device " + ex.ToString());

			}
		}
		public List<DeviceStatusPool> GetDevicePool()
		{
			return _devices;
		}
		private void SetupCleanUpTask()
		{
			//LogWriter.LogWrite("enter into setupclenuptask");

			Task.Run(async () =>
			{
				while (true)
				{
					try
					{
						if (DateTime.Now > lastprocessdate.AddMinutes(5)) await CheckStatus();
						foreach (var item in _devices.Where(x => x.LastHeartBeat.AddMinutes(2) < DateTime.Now))
						{
							item.IsRunning = false;
						}
						//LogWriter.LogWrite("Before CheckDeviceAddition call \n _devices data" + _devices);

						await CheckDeviceAddition();
						var data = new DeviceStatusDto
						{
							TotalDevices = totaldevices,
							RunningDevices = runningdevices
						};
						//LogWriter.LogWrite("after data in SetupCleanUpTask" + data);

						//var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
						//var message = Encoding.UTF8.GetString(body);
						await _buscontrol.CreateExchange<DeviceStatusDto>("dashboarddata", data);

					}
					catch (Exception ex)
					{

						LogWriter.LogWrite("DeviceLog Error " + ex.Message);
					}


					await Task.Delay(120000);
				}
			});
		}
		public async Task TriggerFeedback(string machineid)
		{
			try
			{
				await _buscontrol.CreateExchange<string>("feedbackdata", machineid);
			}
			catch (Exception ex)
			{

				//LogWriter.LogWrite("enter into catch block of TriggerFeedback" + ex.Message);

			}
		}
		private async Task CheckStatus()
		{
			if (DateTime.Now < lastprocessdate.AddDays(-1) || processing) return;
			lastprocessdate = DateTime.Now;
			processing = true;
			try
			{
				//LogWriter.LogWrite("enter into try block of checkstatus");
				using (var scope = _serviceScopeFactory.CreateScope())
				{
					var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWorks>();
					var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
					try
					{
						HttpClient httpClient = new HttpClient();
						var credentials = new
						{
							login = config["consoleuser"],
							password = config["consolepassword"]
						};
						//LogWriter.LogWrite("credentials: " + credentials);
						var curdatetime = DateTime.Now;
						var response = await httpClient.PostAsJsonAsync(config["ApiEndpoints:Authentication"], credentials);
						var token = await response.Content.ReadAsStringAsync();

						httpClient = new HttpClient();
						long todaymilliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
						long yestermilliseconds = DateTimeOffset.Now.AddDays(-10).ToUnixTimeMilliseconds();


						//var url = config["ApiEndpoints:GetTickets"].Replace("169281540000", (LastGetTickets.Ticks / TimeSpan.TicksPerMillisecond).ToString()).Replace("1692901799999", (curdatetime.Ticks / TimeSpan.TicksPerMillisecond).ToString());
						var url = config["ApiEndpoints:GetTickets"].Replace("169281540000", yestermilliseconds.ToString()).Replace("1692901799999", todaymilliseconds.ToString());

						//LogWriter.LogWrite("Url:-" + url);
						//Add lwwsso token 
						httpClient.DefaultRequestHeaders.Add("Cookie", "LWSSO_COOKIE_KEY=" + token);
						response = await httpClient.GetAsync(url);
						//LogWriter.LogWrite("Before ReadfromJsonAsync");

						var ticketrecords = await response.Content.ReadFromJsonAsync<TicketResponseGet>();

						//LogWriter.LogWrite("After ReadfromJsonAsync");

						//LogWriter.LogWrite("ticketrecords : " + ticketrecords);

						var feedbacksheduledata = await uow.csatsetting.GetAllAsync();

						foreach (var ticketrecord in ticketrecords.entities)
						{
							if (ticketrecord.properties.CurrentStatus_c == "Resolved_c")
							{
								//LogWriter.LogWrite("Enter into If Condition after ticketrecord");
								//LogWriter.LogWrite("ticketrecord" + ticketrecord);
								//LogWriter.LogWrite("----------------------------------------------------------------------------------------------------------");

								//LogWriter.LogWrite("ticketId" + ticketrecord.properties.Id);

								var tr = uow.ticketrecord.GetFirstOrDefault(x => x.TicketId == Convert.ToInt32(ticketrecord.properties.Id));

								//LogWriter.LogWrite("tr record:\n" + tr);

								if (tr != null && tr.TicketStatus == 1)
								{
									//LogWriter.LogWrite("Enter into If Condition after var tr");
									//LogWriter.LogWrite("" + ticketrecord.properties.ResolvedTime_c);

									tr.TicketStatus = 2;//resolved
														//tr.ResolvedDateTime = ticketrecord.properties.ResolvedTime_c;
									tr.ResolvedDateTime = ticketrecord.properties.ResolvedTime_c.ToString();
									if (feedbacksheduledata != null)
									{
										var csatsettime = feedbacksheduledata.Where(x => x.Id == 2).FirstOrDefault();
										tr.NextFeedBackSchedule = DateTime.Now.AddMinutes(csatsettime!.FeedbackPopupTime);
									}
									tr.Description = ticketrecord.properties != null ? ticketrecord.properties.DisplayLabel : "";
									tr.Priority = ticketrecord.properties != null ? ticketrecord.properties.Priority : "";
									tr.CurrentStatus_c = ticketrecord.properties != null ? ticketrecord.properties.CurrentStatus_c : "";
									//tr.DisplayLabel = ticketrecord.related_properties!.RegisteredForDevice_C != null ? ticketrecord.related_properties.RegisteredForDevice_C.DisplayLabel : "";
									//tr.SystemId = ticketrecord.related_properties.RegisteredForDevice_C != null ? ticketrecord.related_properties.RegisteredForDevice_C.DisplayLabel : "";
									tr.AssignedTo = ticketrecord.related_properties!.ExpertAssignee != null ? ticketrecord.related_properties.ExpertAssignee.Name : "";
									tr.ExpertAssigneeName = ticketrecord.related_properties!.ExpertAssignee != null ? ticketrecord.related_properties.ExpertAssignee.Name : "";
									tr.RequestedByPersonName = ticketrecord.related_properties!.ContactPerson != null ? ticketrecord.related_properties.ContactPerson.Name : "";
									tr.Location = ticketrecord.related_properties!.RegisteredForLocation != null ? ticketrecord.related_properties.RegisteredForLocation.Name : "";
									tr.RegisteredForLocation = ticketrecord.related_properties!.RegisteredForLocation != null ? ticketrecord.related_properties.RegisteredForLocation.Name : "";
									uow.ticketrecord.Update(tr);
									uow.Save();
								}
								else
								{
									//LogWriter.LogWrite("Enter into else Condition after var tr");
									if (tr == null)
									{
										TicketRecord mapdata = new();
										var MFticketNo = ticketrecord.properties.Id;
										mapdata.TicketId = Convert.ToInt32(MFticketNo);
										mapdata.ONGCSUB_c = 2260734;// Convert.ToInt32(ticketrecord.properties. .ONGCSUB_c); 
										mapdata.ONGCCAT_c = 2260593;// Convert.ToInt32(input.entities[0].properties.ONGCCAT_c); 
										mapdata.Description = ticketrecord.properties != null ? ticketrecord.properties.DisplayLabel : "";
										mapdata.Priority = ticketrecord.properties != null ? ticketrecord.properties.Priority : "";

										//mapdata.RegisteredForDevice_c = Convert.ToInt32(ticketrecord.properties.RegisteredForDevice_c);
										//mapdata.ContactPerson = Convert.ToInt32(ticketrecord.related_properties.ContactPerson.Name);
										mapdata.TicketStatus = 1;//resolved
																 //mapdata.ResolvedDateTime = ticketrecord.properties.ResolvedTime_c;
										mapdata.ResolvedDateTime = ticketrecord.properties!.ResolvedTime_c.ToString();
										if (feedbacksheduledata != null)
										{
											var csatsettime = feedbacksheduledata.Where(x => x.Id == 2).FirstOrDefault();
											mapdata.NextFeedBackSchedule = DateTime.Now.AddMinutes(csatsettime!.FeedbackPopupTime);
										}

										mapdata.AssignedTo = ticketrecord.related_properties!.ExpertAssignee != null ? ticketrecord.related_properties.ExpertAssignee.Name : "";
										mapdata.ExpertAssigneeName = ticketrecord.related_properties!.ExpertAssignee != null ? ticketrecord.related_properties.ExpertAssignee.Name : "";
										mapdata.DisplayLabel = ticketrecord.related_properties.RegisteredForDevice_C != null ? ticketrecord.related_properties.RegisteredForDevice_C.DisplayLabel : "";
										mapdata.SystemId = ticketrecord.related_properties.RegisteredForDevice_C != null ? ticketrecord.related_properties.RegisteredForDevice_C.DisplayLabel : "";
										mapdata.Location = ticketrecord.related_properties!.RegisteredForLocation != null ? ticketrecord.related_properties.RegisteredForLocation.Name : "";
										mapdata.RegisteredForLocation = ticketrecord.related_properties!.RegisteredForLocation != null ? ticketrecord.related_properties.RegisteredForLocation.Name : "";
										mapdata.RequestedByPersonName = ticketrecord.related_properties!.ContactPerson != null ? ticketrecord.related_properties.ContactPerson.Name : "";
										await uow.ticketrecord.AddAsync(mapdata);
										await uow.SaveAsync();
									}
								}
							}
						}


						//Taking Status completed
						uow.ClearTracker();
						lastGetTickets = curdatetime;
						//LogWriter.LogWrite("Get Tickets Records Completed");
					}
					catch (Exception ex)
					{

						//LogWriter.LogWrite("Get Tickets Records Error " + ex.Message);
					}



					foreach (var item in uow.ticketrecord.GetSelectedNoTracking(x => x.DisplayLabel, x => x.TicketStatus == 2 && x.NextFeedBackSchedule <= DateTime.Now).Distinct())
					{

						//LogWriter.LogWrite("check device log" + item);
						if (item != null && item.Length > 0)
						{
							await _buscontrol.CreateExchange<string>("feedbackdata", item);
							Thread.Sleep(100);
						}
						//await _buscontrol.SendAsync<string>("feedbackdata", item);


					};
					processing = false;
				}

			}
			catch (Exception ex)
			{
				//LogWriter.LogWrite("enter into catch block of checkstatus " + ex);
				//LogWriter.LogWrite("enter into catch block of checkstatus " + ex.Message);

				processing = false;

			}

		}

	}
}
