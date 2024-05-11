using System.Text;
using CIPLV2.Frontdesk.Components.Dto;
using CIPLV2.Models.Processing;
using CIPLV2.Models.Registration;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Radzen;

namespace CIPLV2.Frontdesk.Components.Pages
{
	public partial class SystemInfo
	{
		Variant variant = Variant.Filled;
		IEnumerable<DeviceDetailsDtos> _systeminfo { get; set; }

		bool loading { get; set; } = true;
		IEnumerable<int> pageSizeOptions = new int[] { 10, 20, 30 };
		List<DeviceStatusPool> devicedata { get; set; } = new List<DeviceStatusPool>();
		List<DeviceStatusPool> onlinedevicedata { get; set; } = new List<DeviceStatusPool>();
		List<DeviceStatusPool> offlinedevicedata { get; set; } = new List<DeviceStatusPool>();

		protected override async Task OnInitializedAsync()
		{
			if (sessionData.UserData == null || sessionData.UserData.UserName == null)
				NavigationManager.NavigateTo("/");

			await base.OnInitializedAsync();

			await LoadData();
			await DeviceStatusData();

		}
		private async Task LoadData()
		{
			var output = await ciplapiservice.GetData("/api/devicedetails/device");
			if (output != null && output.Status == "Success")
			{
				_systeminfo = JsonConvert.DeserializeObject<IEnumerable<DeviceDetailsDtos>>(output.Data.ToString());
				loading = false;
				//_systeminfo.ToList().ForEach(s =>
				//{
				//	if (s.properties.IsDeleted == false)
				//	{
				//		s.properties.Status = "Running";
				//	}
				//	else
				//	{
				//		s.properties.Status = "Offline";
				//	}
				//});
			}
		}
		private async Task DownloadCsv()
		{
			try
			{
				List<DeviceDetailsDtoss> datas = new List<DeviceDetailsDtoss>();
				_systeminfo.ToList().ForEach(x =>
				{
					DeviceDetailsDtoss data = new DeviceDetailsDtoss();
					data.Id = x.properties.Id;
					data.IsDeleted = x.properties.IsDeleted;
					data.entity_type = x.entity_type;
					data.SubType = x.properties.SubType;
					//data.LastUpdateTime = x.properties.LastUpdateTime;
					data.DisplayLabel = x.properties?.DisplayLabel;
					data.MfDeviceId = x.properties?.MfDeviceId;
					data.IpAddress = x.properties?.IpAddress;
					data.MacAddress = x.properties?.MacAddress;
					data.AgentVersion = x.properties?.AgentVersion;
					data.Location = x.properties?.Location;
					data.Status = x.properties?.Status;


					// Convert long to DateTime
					if (x.properties.LastUpdateTime.ToString().Length <= 10)
					{
						DateTimeOffset resolvedDateTime = DateTimeOffset.FromUnixTimeSeconds(x.properties.LastUpdateTime).UtcDateTime.AddHours(5).AddMinutes(30);

						data.LastUpdateTime = resolvedDateTime.DateTime;
					}
					else
					{
						DateTimeOffset resolvedDateTime = DateTimeOffset.FromUnixTimeMilliseconds(x.properties.LastUpdateTime).UtcDateTime.AddHours(5).AddMinutes(30);

						data.LastUpdateTime = resolvedDateTime.DateTime;
					}
					datas.Add(data);



				});



				// Generate CSV content based on ticketData
				var csvContent = GenerateCsvContent(datas);

				// Prepare the file content for download
				var data = Encoding.UTF8.GetBytes(csvContent);

				// Initiate the file download using FileSaver
				await JSRuntime.InvokeVoidAsync("saveAsFile", "UserInfoReport.csv", data, "text/csv");
				//ticketData = ticketData1;
			}
			catch (Exception ex) { }
		}

		private string GenerateCsvContent(IEnumerable<DeviceDetailsDtoss> data)
		{
			// Generate CSV content based on your data structure
			StringBuilder csvContent = new StringBuilder();

			// Add header row
			var headers = typeof(DeviceDetailsDtoss).GetProperties().Select(property => property.Name);
			csvContent.AppendLine(string.Join(",", headers));

			// Add data rows
			foreach (var item in data)
			{
				var values = typeof(DeviceDetailsDtoss).GetProperties().Select(property => property.GetValue(item)?.ToString() ?? "");
				csvContent.AppendLine(string.Join(",", values));
			}

			return csvContent.ToString();
		}

		private async Task DeviceStatusData()
		{
			try
			{
				var output = await ciplapiservice.GetData("/api/devicedetails/getdevicestatus");
				if (output != null && output.Status == "Success")
				{
					devicedata = JsonConvert.DeserializeObject<List<DeviceStatusPool>>(output.Data.ToString());
					onlinedevicedata = devicedata.FindAll(x => x.IsRunning == true).ToList();
					offlinedevicedata = devicedata.FindAll(x => x.IsRunning == false).ToList();
					foreach (var dev in _systeminfo)
					{
						foreach (var rundev in onlinedevicedata)
						{
							if (rundev.DeviceId.ToLower().Equals(dev.properties.DisplayLabel!.ToLower()))
							{
								dev.properties.Status = "Running";
							}
						}
					}

					foreach (var dev in _systeminfo)
					{
						foreach (var rundev in offlinedevicedata)
						{
							if (rundev.DeviceId.ToLower().Equals(dev.properties.DisplayLabel!.ToLower()))
							{
								dev.properties.Status = "Offline";
							}
						}
					}
				}
			}
			catch (Exception ex) { }
		}
	}
}
