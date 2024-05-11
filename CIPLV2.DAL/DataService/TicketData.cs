using AutoMapper;
using CIPLV2.DAL.Processes;
using CIPLV2.DAL.Unitofworks;
using CIPLV2.Models.Admin;
using CIPLV2.Models.Tickets;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using CIPLV2.Models.PersonDetail;
using Azure;
using Response = CIPLV2.Models.Admin.Response;
using CIPLV2.Models.DeviceDetail;
using Newtonsoft.Json.Linq;
namespace CIPLV2.DAL.DataService
{
	public class TicketData : ITicketData
	{

		readonly IUnitOfWorks _uow;
		readonly IMapper _mapper;
		readonly IConfiguration _config;

		DateTime lastprocessdate = DateTime.Now;
		DateTime LastGetTickets = DateTime.Now.AddDays(-1);
		int lastid = 0;
		readonly IServiceScopeFactory _serviceScopeFactory;
		bool processing = false;
		public TicketData(IUnitOfWorks uow, IMapper mapper, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
		{
			_uow = uow;
			_mapper = mapper;
			_config = configuration;
			_serviceScopeFactory = serviceScopeFactory;
		}


		public async Task<Response> AddTicket(RaiseAgentTicketDTO input)
		{
			Response response = new Response();
			TicketRecord mapdata = new();
			var mapMFdata = _mapper.Map<RaiseMFTicketDTO>(input);
			//LogWriter.LogWrite("json:- " + input);
			try
			{
				var MFticketNo = await SendTicketToMF(mapMFdata);
				if (MFticketNo != null && MFticketNo != "")
				{
					mapdata.TicketId = Convert.ToInt32(MFticketNo);

					//string ticketurl = "https://ithelpdesknew.ongc.co.in/rest/748960833/ems/Incident?filter=(Id+eq" + mapdata.TicketId.ToString() + ")&layout=Id,DisplayLabel,Priority,RegisteredForActualService,ContactPerson.Name,ExpertAssignee,ExpertAssignee.Name,RegisteredForLocation.Name,CurrentStatus_c,RegisteredForDevice_c.DisplayLabel,ResolvedTime_c";
					////getToken.LoginToken;
					//LogWriter.LogWrite("ticketurl: " + ticketurl);
					//LogWriter.LogWrite("token after mfticketno: " + getToken.LoginToken);
					//HttpClient httpClient = new HttpClient();
					//string cookieHeader = $"LWSSO_COOKIE_KEY={getToken.LoginToken}; JSESSIONID=10285831C4953662FFD6714488B00982";

					////Add lwwsso token 
					//httpClient.DefaultRequestHeaders.Add("Cookie", cookieHeader);

					//var response1 = await httpClient.GetAsync(ticketurl);
					//LogWriter.LogWrite("" + response1);
					//var ticketrecords = await response1.Content.ReadFromJsonAsync<TicketResponseGet>();

					//ticketrecords!.entities!.ForEach(x =>
				 //  {
					//   mapdata.AssignedTo = x.related_properties != null ? x.related_properties.ExpertAssignee!.Name : "";
					//   mapdata.ExpertAssignee = x.properties != null ? x.properties!.ExpertAssignee : "";
					//   mapdata.ExpertAssigneeName = x.related_properties!.ExpertAssignee != null ? x.related_properties.ExpertAssignee.Name : "";
					//   mapdata.EntityType = x.entity_type;
					//   mapdata.CurrentStatus_c = x.properties != null ? x.properties.CurrentStatus_c : "";
					//   mapdata.RegisteredForLocation = x.related_properties.RegisteredForLocation != null ? x.related_properties.RegisteredForLocation.Name : "";
					//   mapdata.Location = x.related_properties.RegisteredForLocation != null ? x.related_properties.RegisteredForLocation.Name : "";
					//   mapdata.RequestedByPersonName = x.related_properties.ContactPerson != null ? x.related_properties.ContactPerson.Name : "";
					//   mapdata.TicketStatus = 1;
				 //  });


					mapdata.ONGCSUB_c = Convert.ToInt32(input.entities[0].properties.ONGCSUB_c);

					mapdata.ONGCCAT_c = Convert.ToInt32(input.entities[0].properties.ONGCCAT_c);
					mapdata.ONGCAREA_c = Convert.ToInt32(input.entities[0].properties.ONGCAREA_c);

					mapdata.Description = input.entities[0].properties.Description;

					mapdata.RegisteredForDevice_c = Convert.ToInt32(input.entities[0].properties.RegisteredForDevice_c);

					mapdata.ContactPerson = Convert.ToInt32(input.entities[0].properties.ContactPerson);
					mapdata.DisplayLabel = input.entities[0].properties.SystemId;
					mapdata.Priority = input.entities[0].properties.Priority;
					mapdata.RequestedByPerson = input.entities[0].properties.RequestedByPerson;
					mapdata.SystemId = input.entities[0].properties.SystemId;
					//LogWriter.LogWrite("tickrecord Data:" + mapdata);

					await _uow.ticketrecord.AddAsync(mapdata);
					await _uow.SaveAsync();

					response.Status = "Success";
					response.Message = "Data Saved";
					response.Data = MFticketNo;
				}
				else
				{
					response.Status = "Failed";
					response.Message = "Data Not Saved";
					response.Data = null;
				}
			}
			catch (Exception ex)
			{
				//LogWriter.LogWrite("Error:" + ex);
				response.Status = "Failed";
				var errormessage = await _uow.AddException(ex);
				response.Message = errormessage;

			}
			return response;
		}

		public async Task<Response> GetList()
		{
			Response response = new Response();
			try
			{
				var dbdata = _uow.ticketrecord
					.GetSelectedNoTracking(x => _mapper.Map<TicketRecord>(x));
				response.Status = "Success";
				response.Message = "Data Sent";
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
		public async Task<Response> GetTicketsBydate(DateTime date1, DateTime date2)
		{
			Response response = new Response();
			try
			{
				//long ticks1 = date1.Ticks;
				//long ticks2 = date2.Ticks;
				//var dates1 = ticks1.ToString();
				//var dates2 = ticks2.ToString();

				var dbdata = await _uow.ticketrecord.GetAllAsync(x => x.CreatedDateTime >= date1 && x.CreatedDateTime <= date2);
				response.Status = "Success";
				response.Message = "Data Sent";
				response.Data = dbdata!.ToList();
			}
			catch (Exception ex)
			{
				response.Status = "Failed";
				var errormessage = await _uow.AddException(ex);
				response.Message = errormessage;

			}
			return response;
		}
		public async Task<Response> GetTicketList(string systemid)
		{
			Response response = new Response();
			try
			{
				//var dbdata = _uow.ticketrecord
				//	.GetSelectedNoTracking(x => _mapper.Map<TicketRecord>(x))!
				//	.Where(x => x.SystemId.ToLower() == systemid.ToLower()).ToList();
				//var feedbacksheduledata = await _uow.csatsetting.GetAllAsync();

				var dbdata = await _uow.ticketrecord.GetAllAsync(x => x.SystemId.ToLower() == systemid.ToLower() && x.TicketStatus == 2 && x.NextFeedBackSchedule<=DateTime.Now);
				
				if (dbdata != null&& dbdata.Count()>0)
				{
					
					//dbdata!.ToList().ForEach(x =>
					//{
					//	if (feedbacksheduledata != null)
					//	{
					//		var csatsettime = feedbacksheduledata.FirstOrDefault();
					//		x.NextFeedBackSchedule = DateTime.Now.AddMinutes(csatsettime!.FeedbackPopupTime);
					//	}
					//	//x.NextFeedBackSchedule = DateTime.Now.AddYears(1);
					//	_uow.ticketrecord.Update(x);
					//	_uow.Save();
					//});
					response.Status = "Success";
					response.Message = "Data Sent";
					response.Data = dbdata.ToList();
				}
				else
				{
					response.Status = "Failed";
					response.Message = "Data Not Sent";
					response.Data = null;
				}
			}
			catch (Exception ex)
			{
				//LogWriter.LogWrite("exception in GetTicketList" + ex.Message);
				response.Status = "Failed";
				var errormessage = await _uow.AddException(ex);
				response.Message = ex.Message;
				response.Data = null;
			}
			return response;
		}
		public async Task<Response> UpdateTicket(UpdateTicket updateTicket)
		{
			List<TicketRecord> ticData = new List<TicketRecord>();
			Response response = new Response();
			try
			{
				var dbdata = _uow.ticketrecord.GetFirstOrDefault(x => x.TicketId == updateTicket.IncidentId);
				var feedbacksheduledata = await _uow.csatsetting.GetAllAsync();
				if (dbdata != null)
				{
					if (updateTicket.action == "close")
					{
						dbdata.FeedbackCount += 1;
						dbdata.close_count += 1;
						if (feedbacksheduledata != null)
						{
							var csatsettime = feedbacksheduledata.Where(x => x.Id == 1).FirstOrDefault();
							dbdata.NextFeedBackSchedule = DateTime.Now.AddMinutes(csatsettime!.FeedbackPopupTime);
						}
						//else
						//{
						//	dbdata.NextFeedBackSchedule = DateTime.Now.AddYears(1);
						//}
						//dbdata.NextFeedBackSchedule = DateTime.Now.AddHours(2);
					}
					if (updateTicket.action == "submit")
					{
						dbdata.TicketStatus = 3;
						dbdata.FeedBackRemark = updateTicket.Remarks;
						dbdata.starcount = updateTicket.starcount;
					}
					_uow.ticketrecord.Update(dbdata);
					_uow.Save();
					var ticDatadb = await _uow.ticketrecord.GetAllAsync(x => x.SystemId.ToLower() == dbdata.SystemId.ToLower() && x.TicketId != updateTicket.IncidentId && x.TicketStatus ==2 && x.NextFeedBackSchedule <= DateTime.Now);
					ticData = ticDatadb.ToList();
					response.Status = "Success";
					response.Message = "Data Saved";
					response.Data = ticData;

				}
				else
				{
					response.Status = "Completed";
					response.Message = "";
					//response.Data = null;
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

		private async Task<string> SendTicketToMF(RaiseMFTicketDTO ticket)
		{
			var MFticketNo = "";
			string Authenticate_URL = "https://ithelpdesknew.ongc.co.in/auth/authentication-endpoint/authenticate/token?TENANTID=748960833";
			try
			{
				string requestData = "{\"login\":\"" + "chat" + "\", \"password\":\"" + "Welcome@1234" + "\"}";
				byte[] requestDataBytes = Encoding.UTF8.GetBytes(requestData);
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Authenticate_URL);
				//LogWriter.LogWrite("after MF api call");
				request.Method = "POST";
				request.ContentType = "application/json";
				request.ContentLength = requestDataBytes.Length;

				// Write the request data to the request stream
				using (Stream requestStream = request.GetRequestStream())
				{
					requestStream.Write(requestDataBytes, 0, requestDataBytes.Length);
				}
				using (HttpWebResponse webresponse = (HttpWebResponse)request.GetResponse())
				{
					if (webresponse.StatusCode == HttpStatusCode.OK)
					{
						StreamReader streamReader = new StreamReader(webresponse.GetResponseStream());
						string responseBody = streamReader.ReadToEnd();
						getToken.LoginToken = responseBody;
						//LogWriter.LogWrite("print Token");
						//LogWriter.LogWrite(responseBody);

					}
				}

				//Post data to MF
				HttpClient client = new HttpClient();
				string cookieHeader = $"LWSSO_COOKIE_KEY={getToken.LoginToken}; JSESSIONID=10285831C4953662FFD6714488B00982";
				//LogWriter.LogWrite(cookieHeader);

				client.DefaultRequestHeaders.Add("Cookie", cookieHeader);

				var jsonTicket = JsonConvert.SerializeObject(ticket);

				//LogWriter.LogWrite("serilize tiket" + jsonTicket);
				var httpContent = new StringContent(jsonTicket, Encoding.UTF8, "application/json");

				var response = await client.PostAsync(_config["ApiEndpoints:RaiseTicket"], httpContent);

				//LogWriter.LogWrite("Response Content: " + response.Content);

				var jsonResponse = await response.Content.ReadAsStringAsync();
				//LogWriter.LogWrite("jsonResponse: " + jsonResponse);

				var deserializedResponse = JsonConvert.DeserializeObject(jsonResponse);
				//LogWriter.LogWrite("Deserialized Response: " + deserializedResponse);

				var generatedTicket = JsonConvert.DeserializeObject<Data>(jsonResponse);
				//LogWriter.LogWrite("Deserialized Data: " + generatedTicket);

				if (generatedTicket != null && generatedTicket.entity_result_list.Count > 0)
				{
					MFticketNo = generatedTicket.entity_result_list[0].entity.properties.Id;
					//LogWriter.LogWrite("Ticket ID: " + MFticketNo);
				}
				else
				{
					//LogWriter.LogWrite("Unable to retrieve ticket ID from the response.");
				}
				return MFticketNo;
			}
			catch (Exception ex)
			{
				//LogWriter.LogWrite(ex.Message);
			}
			return MFticketNo;
		}
		public async Task<Response> GetTicketByMFList()
		{
			Response response1 = new Response();
			//if (DateTime.Now < lastprocessdate.AddDays(-1) || processing) return response;
			lastprocessdate = DateTime.Now;
			processing = true;
			try
			{
				using (var scope = _serviceScopeFactory.CreateScope())
				{

					var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWorks>();
					var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
					HttpClient httpClient = new HttpClient();
					var credentials = new
					{
						login = config["consoleuser"],
						password = config["consolepassword"]
					};
					//LogWriter.LogWrite("authentication" + config["ApiEndpoints:Authentication"]);
					//LogWriter.LogWrite("Getticketurl" + config["ApiEndpoints:GetTickets"]);
					var response = await httpClient.PostAsJsonAsync(config["ApiEndpoints:Authentication"], credentials);
					var token = await response.Content.ReadAsStringAsync();

					string cookieHeader = $"LWSSO_COOKIE_KEY={token}; JSESSIONID=10285831C4953662FFD6714488B00982";

					//LogWriter.LogWrite(token);
					httpClient = new HttpClient();
					var curdatetime = DateTime.Now;
					//long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
					//long millisecondsyesterday = DateTime.Now.AddDays(-1).Ticks / TimeSpan.TicksPerMillisecond;
					DateTime currentTime = DateTime.UtcNow;
					DateTime yesterdayTime = DateTime.UtcNow.AddDays(-1);

					long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();
					long yestunixTime = ((DateTimeOffset)yesterdayTime).ToUnixTimeSeconds();

					long todaymilliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
					long yestermilliseconds = DateTimeOffset.Now.AddDays(-1).ToUnixTimeMilliseconds();


					//var url = config["ApiEndpoints:GetTickets"].Replace("169281540000", (LastGetTickets.Ticks / TimeSpan.TicksPerMillisecond).ToString()).Replace("1692901799999", (curdatetime.Ticks / TimeSpan.TicksPerMillisecond).ToString());
					var url = config["ApiEndpoints:GetTickets"].Replace("169281540000", yestermilliseconds.ToString()).Replace("1692901799999", todaymilliseconds.ToString());
					//Add lwwsso token 
					//LogWriter.LogWrite("Url:-" + url);

					httpClient.DefaultRequestHeaders.Add("Cookie", cookieHeader);
					//httpClient.DefaultRequestHeaders.Add("Cookie", "LWSSO_COOKIE_KEY=" + token);
					response = await httpClient.GetAsync(url);
					//LogWriter.LogWrite("" + response.Content);
					//var ticketrecordsdynmic = await response.Content.ReadFromJsonAsync<dynamic>();
					//LogWriter.LogWrite("ticketrecordsdynmic" + ticketrecordsdynmic);

					var ticketrecords = await response.Content.ReadFromJsonAsync<TicketResponseGet>();
					//LogWriter.LogWrite("" + ticketrecords);
					response1.Data = ticketrecords;



					//Taking Status completed
					//uow.ClearTracker();
					LastGetTickets = curdatetime;

					processing = false;
				}

			}
			catch (Exception ex)
			{
				//LogWriter.LogWrite("" + ex.Message);
				//LogWriter.LogWrite("" + ex);
				processing = false;

			}

			return response1;
		}
		public async Task<Response> uploadTicket(List<TicketRecord> tickets)
		{
			Response response = new Response();
			//TicketRecord mapdata = new();
			try
			{
				var dbdata = await _uow.ticketrecord.GetAllAsync();

				var mappedIds = tickets.Select(a => a.TicketId);
				var dbIds = dbdata.Select(a => a.TicketId);

				var commonIdsInDb = mappedIds.Intersect(dbIds).ToList();
				var commonObjectsInMappedData = tickets.Where(a => commonIdsInDb.Contains(a.TicketId)).ToList();


				var idsNotInDb = mappedIds.Except(dbIds).ToList();

				var uncommonObjectsInMappedData = tickets.Where(a => idsNotInDb.Contains(a.TicketId)).ToList();

				if (commonObjectsInMappedData.Any())
				{
					//_uow.ticketrecord.UpdateRange(commonObjectsInMappedData);
				}
				else
				{
					_uow.ticketrecord.AddRange(uncommonObjectsInMappedData);
				}
				await _uow.SaveAsync();

				response.Status = "Success";
				response.Message = "Data Saved";
				response.Data = dbdata;
			}
			catch (Exception ex)
			{
				//LogWriter.LogWrite("Error:" + ex);
				response.Status = "Failed";
				var errormessage = await _uow.AddException(ex);
				response.Message = errormessage;

			}
			return response;
		}


		public async Task<Response> SavePersonDetails()
		{
			Response response1 = new Response();
			try
			{
				List<PersonDetails> persondetails = new List<PersonDetails>();

				var data = await _uow.personDetails.GetAllAsync();

				if (data != null)
				{
					_uow.personDetails.RemoveRange(data);
				}

				string persondetailurl = "https://ithelpdesknew.ongc.co.in/rest/748960833/ems/Person?layout=EmployeeNumber,Name,LastName,FirstName,Location,Upn&size=36195";
				using (var scope = _serviceScopeFactory.CreateScope())
				{

					var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWorks>();
					var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
					HttpClient httpClient = new HttpClient();
					var credentials = new
					{
						login = config["consoleuser"],
						password = config["consolepassword"]
					};

					var response = await httpClient.PostAsJsonAsync(config["ApiEndpoints:Authentication"], credentials);
					var token = await response.Content.ReadAsStringAsync();

					string cookieHeader = $"LWSSO_COOKIE_KEY={token}; JSESSIONID=10285831C4953662FFD6714488B00982";

					httpClient = new HttpClient();
					var curdatetime = DateTime.Now;
					//Add lwwsso token 
					httpClient.DefaultRequestHeaders.Add("Cookie", cookieHeader);

					response = await httpClient.GetAsync(persondetailurl);
					//LogWriter.LogWrite("" + response);
					var personrecords = await response.Content.ReadFromJsonAsync<Persondata>();
					//LogWriter.LogWrite("" + personrecords);
					personrecords.entities.ForEach(x =>
					{
						PersonDetails person = new PersonDetails();
						person.entity_type = x.entity_type;
						person.IsVIP = x.properties.IsVIP;
						person.EmployeeNumber = x.properties.EmployeeNumber;
						person.FirstName = x.properties.FirstName;
						person.LastName = x.properties.LastName;
						person.Email = x.properties.Email;
						person.Name = x.properties.Name;
						person.Location = x.properties.Location;
						person.Upn = x.properties.Upn;
						person.LastUpdateTime = x.properties.LastUpdateTime;
						person.MfpersonId = x.properties.Id;
						persondetails.Add(person);
					});
					var savedata = await _uow.personDetails.AddRangeAsync(persondetails);
					await _uow.SaveAsync();
					response1.Data = savedata;




				}

			}
			catch (Exception ex)
			{
				response1.Status = "Failed";
				var errormessage = await _uow.AddException(ex);
				response1.Message = errormessage;
			}
			return response1;

		}

		public async Task<Response> SaveDeviceDetails()
		{
			Response response1 = new Response();
			try
			{
				List<DeviceDetails> devicedetails = new List<DeviceDetails>();
				var data = await _uow.deviceDetails.GetAllAsync();
				var installeddata = data!.Where(x => x.IsDeleted == false).ToList();

				if (data != null)
				{
					_uow.deviceDetails.RemoveRange(data);
				}

				string devicedetailurl = "https://ithelpdesknew.ongc.co.in/rest/748960833/ems/Device?layout=DisplayLabel,ShortDescription,DefaultWarrantyContract,DefaultMaintenanceContract&size=57740";
				using (var scope = _serviceScopeFactory.CreateScope())
				{

					var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWorks>();
					var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
					HttpClient httpClient = new HttpClient();
					var credentials = new
					{
						login = config["consoleuser"],
						password = config["consolepassword"]
					};


					var response = await httpClient.PostAsJsonAsync(config["ApiEndpoints:Authentication"], credentials);
					var token = await response.Content.ReadAsStringAsync();

					string cookieHeader = $"LWSSO_COOKIE_KEY={token}; JSESSIONID=10285831C4953662FFD6714488B00982";

					httpClient = new HttpClient();
					//Add lwwsso token 
					httpClient.DefaultRequestHeaders.Add("Cookie", cookieHeader);
					response = await httpClient.GetAsync(devicedetailurl);
					//LogWriter.LogWrite("" + response);


					var devicerecords = await response.Content.ReadFromJsonAsync<DeviceJsonObject>();


					devicerecords!.entities.ForEach(x =>
					{
						DeviceDetails device = new DeviceDetails();

						if (installeddata.Select(a => a.MfDeviceId).Contains(x.properties.Id))
						{
							device.IsDeleted = false;
						}
						else
						{
							device.IsDeleted = true;
						}
						device.entity_type = x.entity_type;
						device.DisplayLabel = x.properties.DisplayLabel;
						device.LastUpdateTime = x.properties.LastUpdateTime;
						device.MfDeviceId = x.properties.Id;
						device.SubType = "Desktop";
						devicedetails.Add(device);
					});

					var savedata = await _uow.deviceDetails.AddRangeAsync(devicedetails);
					await _uow.SaveAsync();
					response1.Data = savedata;

				}

			}
			catch (Exception ex)
			{
				response1.Status = "Failed";
				var errormessage = await _uow.AddException(ex);
				response1.Message = errormessage;
			}
			return response1;

		}

		public async Task<Response> SaveStarRating(StarRatingDTO input)
		{
			Response response = new Response();
			try
			{
				var mappeddata = _mapper.Map<StarRating>(input);

				if (mappeddata != null)
				{
					await _uow.starRating.AddAsync(mappeddata);
					await _uow.SaveAsync();
					response.Status = "Success";
					response.Message = "Data Saved";
					response.Data = mappeddata;
				}
				else
				{
					response.Status = "Failed";
					var errormessage = "Data is not valid";
					response.Message = errormessage;

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


		public async Task<Response> SaveTicketDetails()
		{
			Response response1 = new Response();
			try
			{

				for (int i = 4104373; i >= 4000000; i--)
				{


					string ticketurl = "https://ithelpdesknew.ongc.co.in/rest/748960833/ems/Incident?filter=(Id+eq" + i.ToString() + ")&layout=Id,DisplayLabel,Priority,RegisteredForActualService,ContactPerson.Name,ExpertAssignee,ExpertAssignee.Name,RegisteredForLocation.Name,CurrentStatus_c,RegisteredForDevice_c.DisplayLabel,ResolvedTime_c";

					//LogWriter.LogWrite("ticketurl:- " + ticketurl);
					using (var scope = _serviceScopeFactory.CreateScope())
					{

						var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWorks>();
						var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
						HttpClient httpClient = new HttpClient();
						var credentials = new
						{
							login = config["consoleuser"],
							password = config["consolepassword"]
						};

						var response = await httpClient.PostAsJsonAsync(config["ApiEndpoints:Authentication"], credentials);
						var token = await response.Content.ReadAsStringAsync();

						string cookieHeader = $"LWSSO_COOKIE_KEY={token}; JSESSIONID=10285831C4953662FFD6714488B00982";

						httpClient = new HttpClient();
						var curdatetime = DateTime.Now;
						//Add lwwsso token 
						httpClient.DefaultRequestHeaders.Add("Cookie", cookieHeader);

						response = await httpClient.GetAsync(ticketurl);
						//LogWriter.LogWrite("" + response);
						var ticketrecords = await response.Content.ReadFromJsonAsync<TicketResponseGet>();
						//LogWriter.LogWrite("" + ticketrecords);

						ticketrecords!.entities!.ForEach(async x =>
						{
							//LogWriter.LogWrite("Enter into Foreach Loop");
							TicketRecord Ticket = new TicketRecord();
							var tr = uow.ticketrecord.GetFirstOrDefault(y => y.TicketId == Convert.ToInt32(x.properties!.Id));
							if (tr == null)
							{
								//LogWriter.LogWrite("Enter into if condition of foreach loop");
								Ticket.TicketId = Convert.ToInt32(x.properties!.Id);
								Ticket.Description = x.properties.DisplayLabel;
								Ticket.SystemId = x.related_properties!.RegisteredForDevice_C != null ? x.related_properties.RegisteredForDevice_C.DisplayLabel : "";
								if (x.properties.CurrentStatus_c == "Resolved_c")
								{
									Ticket.TicketStatus = 2;
								}
								else
								{
									Ticket.TicketStatus = 1;
								}
								Ticket.CurrentStatus_c = x.properties.CurrentStatus_c;
								Ticket.Description = x.properties.DisplayLabel;
								Ticket.ResolvedDateTime = x.properties.ResolvedTime_c.ToString();
								Ticket.NextFeedBackSchedule = DateTime.Now;
								Ticket.RequestedByPersonName = x.related_properties!.ContactPerson != null ? x.related_properties.ContactPerson.Name : "";
								Ticket.AssignedTo = x.related_properties!.ExpertAssignee != null ? x.related_properties.ExpertAssignee.Name : "";
								Ticket.ExpertAssigneeName = x.related_properties!.ExpertAssignee != null ? x.related_properties.ExpertAssignee.Name : "";
								Ticket.DisplayLabel = x.related_properties!.RegisteredForDevice_C != null ? x.related_properties.RegisteredForDevice_C.DisplayLabel : "";
								Ticket.Location = x.related_properties!.RegisteredForLocation != null ? x.related_properties.RegisteredForLocation.Name : "";
								Ticket.RegisteredForLocation = x.related_properties!.RegisteredForLocation != null ? x.related_properties.RegisteredForLocation.Name : "";
								await uow.ticketrecord.AddAsync(Ticket);
								await uow.SaveAsync();
							}
							else
							{
								//LogWriter.LogWrite("Enter into else condition of foreach loop");

								if (x.properties!.CurrentStatus_c == "Resolved_c")
								{
									tr.TicketStatus = 2;
								}
								else
								{
									tr.TicketStatus = 1;
								}
								tr.Description = x.properties!.DisplayLabel;
								tr.RequestedByPersonName = x.related_properties!.ContactPerson != null ? x.related_properties.ContactPerson.Name : "";
								tr.ResolvedDateTime = x.properties.ResolvedTime_c.ToString();
								tr.NextFeedBackSchedule = DateTime.Now;
								tr.ExpertAssigneeName = x.related_properties!.ExpertAssignee != null ? x.related_properties.ExpertAssignee.Name : "";
								tr.AssignedTo = x.related_properties!.ExpertAssignee != null ? x.related_properties.ExpertAssignee.Name : "";
								tr.Location = x.related_properties!.RegisteredForLocation != null ? x.related_properties.RegisteredForLocation.Name : "";
								tr.RegisteredForLocation = x.related_properties!.RegisteredForLocation != null ? x.related_properties.RegisteredForLocation.Name : "";

								uow.ticketrecord.Update(tr);
								uow.Save();
							}
						});

					}

				}

			}
			catch (Exception ex)
			{
				//LogWriter.LogWrite("exception while saving tickets:\n" + ex.Message);
				response1.Status = "Failed";
				var errormessage = await _uow.AddException(ex);
				response1.Message = errormessage;
			}
			return response1;

		}

		public async Task<Response> GetTicketDataList(string ticId)
		{
			Response response = new Response();
			try
			{
				var dbdata = await _uow.ticketrecord.GetAllAsync(x => x.TicketId.ToString() == ticId);
				if (dbdata != null && dbdata.Count() > 0)
				{
					response.Status = "Success";
					response.Message = "Data Sent";
					response.Data = dbdata.ToList();
				}
				else
				{
					response.Status = "Failed";
					response.Message = "Data Not Sent";
					response.Data = null;
				}
			}
			catch (Exception ex)
			{
				//LogWriter.LogWrite("exception in GetTicketList" + ex.Message);
				response.Status = "Failed";
				var errormessage = await _uow.AddException(ex);
				response.Message = ex.Message;
				response.Data = null;
			}
			return response;
		}


	}
}
