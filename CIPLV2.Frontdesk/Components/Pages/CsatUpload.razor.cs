using CIPLV2.Models.Tickets;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using OfficeOpenXml;
using Radzen;

namespace CIPLV2.Frontdesk.Components.Pages;

public partial class CsatUpload
{
	Variant variant = Variant.Filled;
	List<TicketRecord> ticketRecords = new List<TicketRecord>();
	public IBrowserFile? SelectedFile { get; private set; }
	string docname = string.Empty;
	string selectedFileName { get; set; }
	bool checkColumn = false;
	List<CsatSetting> csatsetting=new List<CsatSetting>();
	bool checkticId = false;
	List<TicketRecord> ticketData { get; set; } = new List<TicketRecord>();

	protected override async Task OnInitializedAsync()
	{
		if (sessionData.UserData == null || sessionData.UserData.UserName == null||sessionData.UserData.UserName=="")
			NavigationManager.NavigateTo("/");

		await GetData();
		await base.OnInitializedAsync();

	}

	public async Task UploadFile()
	{
		try
		{
			await Task.Delay(1000);
			await dialogService.Alert("File uploaded successfully!", "Success");
		}
		catch (Exception ex)
		{

		}

	}
	public async Task GetData()
	{
		var response = await ciplapiservice.GetData("/api/csatsetting");
		if (response != null && response.Status == "Success")
		{
			csatsetting = JsonConvert.DeserializeObject<List<CsatSetting>>(response.Data.ToString());
			StateHasChanged();
		}
		else
		{
			await dialogService.Alert(response.Message, "Error");
		}
	}
	public async Task OnChange(InputFileChangeEventArgs e)
	{
		SelectedFile = e.GetMultipleFiles().FirstOrDefault();
		if (SelectedFile != null)
		{
			docname = SelectedFile.Name;
		}
		//var excelData = await CsatsUploadFile(SelectedFile);
		//if (excelData == new List<TicketRecord>())
		//{
		//	dialogService.Alert("Invalid ExcelSheet");
		//	return;
		//}
		//if (excelData != null && excelData.Count>0)
		//{
		//	await Validate(excelData);
		//}
		//else
		//{
		//	dialogService.Alert("Invalid ExcelSheet");
		//}

	}
	public async Task uploadFile()
	{
		if (SelectedFile != null)
		{
			var excelData = await CsatsUploadFile(SelectedFile);
			if (excelData == new List<TicketRecord>())
			{
				dialogService.Alert("Invalid ExcelSheet");
				docname = "";
				return;
			}
			if (excelData != null && excelData.Count > 0)
			{
				await Validate(excelData);
				docname = "";
			}
			else
			{
				dialogService.Alert("Invalid ExcelSheet");
				docname = "";
			}
		}
		else
		{
			dialogService.Alert("Please Choose File");
			docname = "";

		}
	}
	public async Task Validate(List<TicketRecord> catdata)
	{

		var response = await ciplapiservice.PostData(catdata, "/api/tickets/import");
		if (response != null)
		{
			ticketRecords = JsonConvert.DeserializeObject<List<TicketRecord>>(response.Data.ToString());

			dialogService.Alert("Upload Successfully");

		}

	}
	public async Task<List<TicketRecord>> CsatsUploadFile(IBrowserFile file)
	{
		try
		{
			ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
			using (var stream = new MemoryStream())
			{
				await file.OpenReadStream().CopyToAsync(stream);

				using (var package = new ExcelPackage(stream))
				{
					var worksheet = package.Workbook.Worksheets[0];
					for (int row = 1; row <= 1; row++)
					{
						//if (checkColumn == true)
						//{
						//	//dialogService.Alert("Invalid ExcelSheet");
						//	return new List<TicketRecord>();
						//}
						if (worksheet.Cells[row, 1].Value.ToString().ToLower().Replace(" ", "") != "incidentid*")
						{
							//await dialogService.Alert("Invalid ExcelSheet Column");
							checkColumn = true;
							break;
						}
						if (worksheet.Cells[row, 2].Value.ToString().ToLower().Replace(" ", "") != "severity*")
						{
							//await dialogService.Alert("Invalid ExcelSheet Column");
							checkColumn = true;
							break;
						}
						if (worksheet.Cells[row, 3].Value.ToString().ToLower().Replace(" ", "") != "subject*")
						{
							//await dialogService.Alert("Invalid ExcelSheet Column");
							checkColumn = true;
							break;
						}
						if (worksheet.Cells[row, 4].Value.ToString().ToLower().Replace(" ", "") != "location*")
						{
							//await dialogService.Alert("Invalid ExcelSheet Column");
							checkColumn = true;
							break;
						}
						if (worksheet.Cells[row, 5].Value.ToString().ToLower().Replace(" ", "") != "region*")
						{
							//await dialogService.Alert("Invalid ExcelSheet Column");
							checkColumn = true;
							break;
						}
						if (worksheet.Cells[row, 6].Value.ToString().ToLower().Replace(" ", "") != "username*")
						{
							//await dialogService.Alert("Invalid ExcelSheet Column");
							checkColumn = true;
							break;
						}
						if (worksheet.Cells[row, 7].Value.ToString().ToLower().Replace(" ", "") != "userid*")
						{
							//await dialogService.Alert("Invalid ExcelSheet Column");
							checkColumn = true;
							break;
						}
						if (worksheet.Cells[row, 8].Value.ToString().ToLower().Replace(" ", "") != "hostname*")
						{
							//await dialogService.Alert("Invalid ExcelSheet Column");
							checkColumn = true;
							break;
						}
						if (worksheet.Cells[row, 9].Value.ToString().ToLower().Replace(" ", "") != "engineername*")
						{
							//await dialogService.Alert("Invalid ExcelSheet Column");
							checkColumn = true;
							break;
						}
						if (worksheet.Cells[row, 10].Value.ToString().ToLower().Replace(" ", "") != "status*")
						{
							//await dialogService.Alert("Invalid ExcelSheet Column");
							checkColumn = true;
							break;
						}
						if (worksheet.Cells[row, 11].Value.ToString().ToLower().Replace(" ", "") != "resolutiondate*")
						{
							//await dialogService.Alert("Invalid ExcelSheet Column");
							checkColumn = true;
							break;
						}


					}
					if (checkColumn)
					{
						await dialogService.Alert("Invalid ExcelSheet");
						return new List<TicketRecord>();
					}
					for (int row = 2; row <= worksheet.Dimension.Rows; row++)
					{
						if (worksheet.Cells[row, 1] != null)
						{
							var rowData = new TicketRecord();

							rowData.TicketId = Convert.ToInt32(worksheet.Cells[row, 1].Value);
							rowData.Description = worksheet.Cells[row, 3].Value.ToString();
							rowData.Priority = worksheet.Cells[row, 2].Value.ToString();
							rowData.Severity = worksheet.Cells[row, 2].Value.ToString();
							rowData.RegisteredForLocation = worksheet.Cells[row, 4].Value.ToString();
							rowData.Region = worksheet.Cells[row, 5].Value.ToString();
							rowData.SystemId = worksheet.Cells[row, 8].Value.ToString();
							rowData.DisplayLabel = worksheet.Cells[row, 8].Value.ToString();
							rowData.ExpertAssigneeName = worksheet.Cells[row, 9].Value.ToString();
							rowData.AssignedTo = worksheet.Cells[row, 9].Value.ToString();
							rowData.CurrentStatus_c = worksheet.Cells[row, 10].Value.ToString();
							var resolveddate = Convert.ToDateTime(worksheet.Cells[row, 11].Value);
							var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
							long epochTime = (long)(resolveddate.ToUniversalTime() - unixEpoch).TotalSeconds;

							rowData.ResolvedDateTime = epochTime.ToString();
							rowData.RequestedByPerson = worksheet.Cells[row, 7].Value.ToString();
							rowData.RequestedByPersonName = worksheet.Cells[row, 6].Value.ToString();
							if (csatsetting != null)
							{
								var data = csatsetting.Where(x=>x.Id==2).FirstOrDefault();
								rowData.NextFeedBackSchedule = DateTime.Now.AddMinutes(data!.FeedbackPopupTime);
							}
							rowData.FeedBackReceivedOn = DateTime.Now;
							rowData.LastUpdateTime = DateTime.Now;
							rowData.EntityType = "Incident";
							rowData.TicketGenerated = true;
							rowData.FeedBackReceived = true;
							if (rowData.CurrentStatus_c!.ToLower() == "resolved_c")
							{
								rowData.TicketStatus = 2;
							}
							checkticId = await GetTicketData(rowData.TicketId.ToString());
							if (checkticId == false)
							{
								ticketRecords.Add(rowData);
							}
							else
							{
								return new();
							}
						}
					}

					if (checkticId == true)
					{
						await dialogService.Alert("Invalid ExcelSheet");
						return new List<TicketRecord>();
					}
				}

			}
		}
		catch (Exception ex)
		{

		}
		return ticketRecords;
	}

	private async Task<bool> GetTicketData(string ticid)
	{
		var output = await ciplapiservice.GetData("/api/tickets/ticData?ticId=" + ticid);
		if (output != null && output.Status == "Success")
		{
			ticketData = JsonConvert.DeserializeObject<List<TicketRecord>>(output.Data.ToString());
		}
		else
		{
			ticketData = new();
		}
		if (ticketData.Count == 0 || ticketData == null)
		{
			return false;
		}
		return true;
	}
	//public async Task<List<TicketRecord>> CsatsUploadFile(IBrowserFile file)
	//{
	//	try
	//	{
	//		ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
	//		using (var stream = new MemoryStream())
	//		{
	//			await file.OpenReadStream().CopyToAsync(stream);

	//			using (var package = new ExcelPackage(stream))
	//			{
	//				var worksheet = package.Workbook.Worksheets[0];
	//				for (int row = 1; row <= 1; row++)
	//				{
	//					if (checkColumn == true)
	//					{
	//						dialogService.Alert("Invalid ExcelSheet");
	//						return new List<TicketRecord>();
	//					}
	//					//var rowData = new ManPowerExcelDTO();
	//					if (worksheet.Cells[row, 1].Value.ToString().ToLower().Replace(" ", "") != "incidentid*".ToLower())
	//					{
	//						await dialogService.Alert("Invalid ExcelSheet Column");
	//						checkColumn = true;
	//						break;
	//					}
	//					if (worksheet.Cells[row, 2].Value.ToString().ToLower().Replace(" ", "") != "severity*".ToLower())
	//					{
	//						await dialogService.Alert("Invalid ExcelSheet Column");
	//						checkColumn = true;
	//						break;
	//					}
	//					if (worksheet.Cells[row, 3].Value.ToString().ToLower().Replace(" ", "") != "subject*".ToLower())
	//					{
	//						await dialogService.Alert("Invalid ExcelSheet Column");
	//						checkColumn = true;
	//						break;
	//					}
	//					if (worksheet.Cells[row, 4].Value.ToString().ToLower().Replace(" ", "") != "location*".ToLower())
	//					{
	//						await dialogService.Alert("Invalid ExcelSheet Column");
	//						checkColumn = true;
	//						break;
	//					}
	//					if (worksheet.Cells[row, 5].Value.ToString().ToLower().Replace(" ", "") != "region*".ToLower())
	//					{
	//						await dialogService.Alert("Invalid ExcelSheet Column");
	//						checkColumn = true;
	//						break;
	//					}
	//					if (worksheet.Cells[row, 6].Value.ToString().ToLower().Replace(" ", "") != "username*".ToLower())
	//					{
	//						await dialogService.Alert("Invalid ExcelSheet Column");
	//						checkColumn = true;
	//						break;
	//					}
	//					if (worksheet.Cells[row, 7].Value.ToString().ToLower().Replace(" ", "") != "userid*".ToLower())
	//					{
	//						await dialogService.Alert("Invalid ExcelSheet Column");
	//						checkColumn = true;
	//						break;
	//					}
	//					if (worksheet.Cells[row, 8].Value.ToString().ToLower().Replace(" ", "") != "hostname*".ToLower())
	//					{
	//						await dialogService.Alert("Invalid ExcelSheet Column");
	//						checkColumn = true;
	//						break;
	//					}
	//					if (worksheet.Cells[row, 9].Value.ToString().ToLower().Replace(" ", "") != "engineername*".ToLower())
	//					{
	//						await dialogService.Alert("Invalid ExcelSheet Column");
	//						checkColumn = true;
	//						break;
	//					}
	//					if (worksheet.Cells[row, 10].Value.ToString().ToLower().Replace(" ", "") != "status*".ToLower())
	//					{
	//						await dialogService.Alert("Invalid ExcelSheet Column");
	//						checkColumn = true;
	//						break;
	//					}
	//					if (worksheet.Cells[row, 11].Value.ToString().ToLower().Replace(" ", "") != "resolutiondate*".ToLower())
	//					{
	//						await dialogService.Alert("Invalid ExcelSheet Column");
	//						checkColumn = true;
	//						break;
	//					}


	//				}
	//				if (checkColumn)
	//				{
	//					await dialogService.Alert("Invalid ExcelSheet");
	//					return new List<TicketRecord>();
	//				}
	//				for (int row = 2; row <= worksheet.Dimension.Rows; row++)
	//				{
	//					var rowData = new TicketRecord();

	//					rowData.TicketId = Convert.ToInt32(worksheet.Cells[row, 1].Value);
	//					rowData.Description = worksheet.Cells[row, 3].Value.ToString();
	//					//rowData.DisplayLabel = worksheet.Cells[row, 3].Value.ToString();
	//					rowData.Priority = worksheet.Cells[row, 2].Value.ToString();
	//					rowData.Severity = worksheet.Cells[row, 2].Value.ToString();
	//                       rowData.RegisteredForLocation = worksheet.Cells[row, 4].Value.ToString();
	//					rowData.Region = worksheet.Cells[row, 5].Value.ToString();
	//					rowData.SystemId = worksheet.Cells[row, 8].Value.ToString();
	//                       rowData.DisplayLabel = worksheet.Cells[row, 8].Value.ToString();
	//                       rowData.ExpertAssigneeName = worksheet.Cells[row, 9].Value.ToString();
	//                       rowData.AssignedTo = worksheet.Cells[row, 9].Value.ToString();
	//					rowData.CurrentStatus_c = worksheet.Cells[row, 10].Value.ToString();
	//					//var resolveddate = Convert.ToDateTime(worksheet.Cells[row, 11].Value).ToFileTimeUtc();

	//					////var reso= DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(resolveddate.Ticks)).DateTime;

	//					////                  long ticks = reso.Ticks;
	//					//rowData.ResolvedDateTime = resolveddate.ToString();
	//					var resolveddate = Convert.ToDateTime(worksheet.Cells[row, 11].Value);
	//					var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
	//					long epochTime = (long)(resolveddate.ToUniversalTime() - unixEpoch).TotalSeconds;

	//					rowData.ResolvedDateTime = epochTime.ToString();
	//					rowData.RequestedByPerson = worksheet.Cells[row, 7].Value.ToString();
	//					rowData.RequestedByPersonName = worksheet.Cells[row, 6].Value.ToString();
	//					if (csatsetting != null)
	//					{
	//						var data = csatsetting.FirstOrDefault();
	//						rowData.NextFeedBackSchedule = DateTime.Now.AddMinutes(data!.FeedbackPopupTime);
	//					}
	//					rowData.FeedBackReceivedOn=DateTime.Now;
	//					rowData.LastUpdateTime=DateTime.Now;
	//					rowData.EntityType = "Incident";
	//					rowData.TicketGenerated = true;
	//					rowData.FeedBackReceived = true;
	//					if (rowData.CurrentStatus_c!.ToLower() == "resolved_c")
	//					{
	//						rowData.TicketStatus = 2;
	//					}
	//					ticketRecords.Add(rowData);
	//				}
	//			}

	//		}
	//	}
	//	catch (Exception ex)
	//	{

	//	}
	//	return ticketRecords;
	//}
	private async Task OpenFilePicker()
	{
		// Trigger file input click event
		await JSRuntime.InvokeVoidAsync("openFilePicker", "upload");
	}
}
