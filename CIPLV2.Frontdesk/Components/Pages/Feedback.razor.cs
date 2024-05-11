using CIPLV2.Frontdesk.Components;
using CIPLV2.Models.Tickets;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Radzen;
using System.Text;

namespace CIPLV2.Frontdesk.Components.Pages
{
	public partial class Feedback
	{
		Variant variant = Variant.Filled;
		List<TicketRecord> _feedback { get; set; } = new List<TicketRecord>();
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
			var output = await ciplapiservice.GetData("/api/tickets/getlist");
			if (output != null && output.Status == "Success")
			{
				_feedback = JsonConvert.DeserializeObject<List<TicketRecord>>(output.Data.ToString());
				_feedback = _feedback.Where(x => x.SystemId != "" && x.SystemId != null).ToList();
				loading = false;
			}
		}

		private async Task DownloadCsvFeedback()
		{
			// Generate CSV content based on ticketData
			var csvContent = GenerateCsvContentfeed(_feedback);

			// Prepare the file content for download
			var data = Encoding.UTF8.GetBytes(csvContent);

			// Initiate the file download using FileSaver
			await JSRuntime.InvokeVoidAsync("saveAsFile", "Feedback.csv", data, "text/csv");
			//ticketData = ticketData1;
		}


		private string GenerateCsvContentfeed(IEnumerable<TicketRecord> data)
		{
			// Generate CSV content based on your data structure
			StringBuilder csvContent = new StringBuilder();

			// Add header row
			var headers = typeof(TicketRecord).GetProperties().Select(property => property.Name);
			csvContent.AppendLine(string.Join(",", headers));

			// Add data rows
			foreach (var item in data)
			{
				var values = typeof(TicketRecord).GetProperties().Select(property => property.GetValue(item)?.ToString() ?? "");
				csvContent.AppendLine(string.Join(",", values));
			}

			return csvContent.ToString();
		}
	}
}
