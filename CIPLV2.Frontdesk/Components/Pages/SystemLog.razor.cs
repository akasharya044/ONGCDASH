using Microsoft.JSInterop;
using Newtonsoft.Json;
using Radzen;
using System.Text;

namespace CIPLV2.Frontdesk.Components.Pages
{
	public partial class SystemLog
	{
		Variant variant = Variant.Filled;

		List<DeviceDailyStatusDto> _systemlog { get; set; } = new List<DeviceDailyStatusDto>();
		bool loading { get; set; } = true;
		IEnumerable<int> pageSizeOptions = new int[] { 10, 20, 30 };

		protected override async Task OnInitializedAsync()
		{
			if (sessionData.UserData == null || sessionData.UserData.UserName == null)
				NavigationManager.NavigateTo("/");

			await base.OnInitializedAsync();
			await LoadData();
		}



		private async Task LoadData()
		{
			try
			{
				var output = await ciplapiservice.GetData("api/devicedetails/device/dailystatus");
				if (output != null && output.Status == "Success")
				{
					_systemlog = JsonConvert.DeserializeObject<List<DeviceDailyStatusDto>>(output.Data.ToString());
					loading = false;
					StateHasChanged();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}
		}


		private async Task DownloadCsv()
		{
			// Generate CSV content based on ticketData
			var csvContent = GenerateCsvContentinstalled(_systemlog);

			// Prepare the file content for download
			var data = Encoding.UTF8.GetBytes(csvContent);

			// Initiate the file download using FileSaver
			await JSRuntime.InvokeVoidAsync("saveAsFile", "Sytemlog.csv", data, "text/csv");
			//ticketData = ticketData1;
		}


		private string GenerateCsvContentinstalled(IEnumerable<DeviceDailyStatusDto> data)
		{
			// Generate CSV content based on your data structure
			StringBuilder csvContent = new StringBuilder();

			// Add header row
			var headers = typeof(DeviceDailyStatusDto).GetProperties().Select(property => property.Name);
			csvContent.AppendLine(string.Join(",", headers));

			// Add data rows
			foreach (var item in data)
			{
				var values = typeof(DeviceDailyStatusDto).GetProperties().Select(property => property.GetValue(item)?.ToString() ?? "");
				csvContent.AppendLine(string.Join(",", values));
			}

			return csvContent.ToString();
		}
	}
}
