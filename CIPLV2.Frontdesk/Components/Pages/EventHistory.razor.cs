using CIPLV2.Models.EventHistories;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Radzen;
using System.Text;

namespace CIPLV2.Frontdesk.Components.Pages
{
	public partial class EventHistory
	{
		Variant variant = Variant.Filled;
		List<EventHistoryDto> _event { get; set; } = new List<EventHistoryDto>();
		List<EventHistoryDtoNew> _eventcount { get; set; } = new List<EventHistoryDtoNew>();
		bool loading { get; set; } = true;
		IEnumerable<int> pageSizeOptions = new int[] { 10, 20, 30 };
		DateTime? value;

		List<EventHistoryDto> distinctEventCounts = new List<EventHistoryDto>();
		int count = 0;


		//Dictionary<string, int> distinctEventCounts = new Dictionary<string, int>();
		protected override async Task OnInitializedAsync()
		{
			if (sessionData.UserData == null || sessionData.UserData.UserName == null)
				NavigationManager.NavigateTo("/");

			await base.OnInitializedAsync();
			await LoadData();
			await CountEvenData();
		}
		private async Task LoadData()
		{
			try
			{
				var output = await ciplapiservice.GetData("/api/eventHistory");
				if (output != null && output.Status == "Success")
				{
					_event = JsonConvert.DeserializeObject<List<EventHistoryDto>>(output.Data.ToString());
					_event = _event.DistinctBy(x => x.Event).ToList();

					distinctEventCounts = _event
				.GroupBy(x => x.Event)
				.Select(g => new EventHistoryDto { Event = g.Key, Count = g.Count() })
				.ToList();

					loading = false;
					StateHasChanged();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}
		}


		private async Task ShowEvenList(string EventName)
		{
			NavigationManager.NavigateTo("/eventhistory/" + EventName);
		}


		private async Task DownloadCsvEvent()
		{
			// Generate CSV content based on ticketData
			var csvContent = GenerateCsvContentEvent(_event);

			// Prepare the file content for download
			var data = Encoding.UTF8.GetBytes(csvContent);

			// Initiate the file download using FileSaver
			await JSRuntime.InvokeVoidAsync("saveAsFile", "ChatbotAnalytics.csv", data, "text/csv");
			//ticketData = ticketData1;
		}


		private string GenerateCsvContentEvent(IEnumerable<EventHistoryDto> data)
		{
			// Generate CSV content based on your data structure
			StringBuilder csvContent = new StringBuilder();

			// Add header row
			var headers = typeof(EventHistoryDto).GetProperties().Select(property => property.Name);
			csvContent.AppendLine(string.Join(",", headers));

			// Add data rows
			foreach (var item in data)
			{
				var values = typeof(EventHistoryDto).GetProperties().Select(property => property.GetValue(item)?.ToString() ?? "");
				csvContent.AppendLine(string.Join(",", values));
			}

			return csvContent.ToString();
		}

		public async Task CountEvenData()
		{

			try
			{
				foreach (var item in _event)
				{
					var output = await ciplapiservice.GetData("api/eventHistory/evenname/" + item.Event);
					if (output != null && output.Status == "Success")
					{
						_eventcount = JsonConvert.DeserializeObject<List<EventHistoryDtoNew>>(output.Data.ToString());
						item.Count = _eventcount.Count();
						StateHasChanged();
					}

				}

			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}
		}
	}

}
