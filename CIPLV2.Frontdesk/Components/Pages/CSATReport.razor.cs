using CIPLV2.Models.Category;
using CIPLV2.Models.Tickets;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Radzen;
using Radzen.Blazor;
using System.Formats.Asn1;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace CIPLV2.Frontdesk.Components.Pages
{
	public partial class CSATReport
	{
		DateTime? date1;
		DateTime? date2;
		RadzenDataGrid<TicketRecord> grid;
		int count;
		List<TicketRecord> ticketData { get; set; } =new List<TicketRecord>();
		List<TicketRecord> ticketData1 { get; set; }=new List<TicketRecord>();
        Variant variant = Variant.Filled;

        bool loading { get; set; } = false;
		string searchText = "";

		IEnumerable<int> pageSizeOptions = new int[] { 10, 20, 30 };

		DateTime currentDate = DateTime.Now;
		
		protected override async Task OnInitializedAsync()
		{
			if (sessionData.UserData == null || sessionData.UserData.UserName == null)
				NavigationManager.NavigateTo("/");
			DateTime startDate = new DateTime(currentDate.Year, currentDate.Month, 1);
			DateTime endDate = startDate.AddMonths(1).AddDays(-1);
			await LoadData(startDate, endDate);

			await base.OnInitializedAsync();
		}
		private void OnClick()
		{
			if (date1 == null || date2 == null)
			{
				dialogService.Alert("Please Select Date");

			}
			
			else
			{
				LoadData(date1,date2);
			}
		}
		private async Task LoadData(DateTime? date1,DateTime? date2)
		{
			try
			{
				loading = true;
				//System.Threading.Thread.Sleep(2000);
				var output = await ciplapiservice.GetData("/api/tickets/filter?date1=" + date1 + "&date2=" + date2);
				if (output != null && output.Status == "Success")
				{
					ticketData = JsonConvert.DeserializeObject<List<TicketRecord>>(output.Data.ToString());
					ticketData1 = ticketData.ToList();
				}
				loading = false;

				StateHasChanged();
			}
			catch (Exception ex) { loading = false; }
		}

		private async Task DownloadCsv()
		{
			try
			{
				int srno=0;
				List<TicketReport> reports = new List<TicketReport>();
				ticketData1.ForEach(x =>
				{
					TicketReport report = new TicketReport();
					srno++;
					report.sr_no = srno;
					report.incident_id = x.TicketId;
					report.priority = x.Priority;
					report.engineer_name = x.ExpertAssigneeName;
					report.system_id = x.SystemId;
					report.subject = x.Description;
					report.user_name = x.RequestedByPersonName;
					if (x.TicketStatus == 2)
					{
						report.feedback_status = "pending";

					}
					if (x.TicketStatus == 3)
					{
						report.feedback_status = "completed";

					}
					report.location = x.RegisteredForLocation;
					if (x.starcount > 0)
					{
						report.feedback_rating = x.starcount;
					}
					else
					{
						report.feedback_rating = null;
					}
					report.close_count = x.FeedbackCount;
					report.status = x.CurrentStatus_c;
					report.feedback_comment = x.FeedBackRemark;
					report.created_at = x.CreatedDateTime;

					if (long.TryParse(x.ResolvedDateTime, out long resolvedTicks))
					{
						// Convert long to DateTime
						DateTime resolvedDateTime = DateTimeOffset.FromUnixTimeMilliseconds(resolvedTicks).DateTime;

						report.resolved_date = resolvedDateTime;
						// Format DateTime as string
						}
					else
					{
						report.resolved_date = null;
					}

					reports.Add(report);
				});
				//// Generate CSV content based on ticketData
				var csvContent = GenerateCsvContent(reports);

				// Prepare the file content for download
				var data = Encoding.UTF8.GetBytes(csvContent);

				// Initiate the file download using FileSaver
				await JSRuntime.InvokeVoidAsync("saveAsFile", "CSATReport.csv", data, "text/csv");
				ticketData = ticketData1;
			}
			catch(Exception ex) { }
		}
		private string GenerateCsvContent(IEnumerable<TicketReport> data)
		{
			// Generate CSV content based on your data structure
			StringBuilder csvContent = new StringBuilder();

			// Add header row
			var headers = typeof(TicketReport).GetProperties().Select(property => property.Name);
			csvContent.AppendLine(string.Join(",", headers));

			// Add data rows
			foreach (var item in data)
			{
				var values = typeof(TicketReport).GetProperties().Select(property =>
				{
					var value = property.GetValue(item)?.ToString() ?? "";

					// Replace newlines with \r\n for all columns
					value = value.Replace("\n", "\r\n");

					// Enclose the value in double quotes to handle line breaks within a cell
					value = $"\"{value}\"";

					return value;
				});

				csvContent.AppendLine(string.Join(",", values));
			}

			return csvContent.ToString();
		}
		//private string GenerateCsvContent(IEnumerable<TicketReport> data)
		//{
		//	// Generate CSV content based on your data structure
		//	StringBuilder csvContent = new StringBuilder();

		//	// Add header row
		//	var headers = typeof(TicketReport).GetProperties().Select(property => property.Name);
		//	csvContent.AppendLine(string.Join(",", headers));

		//	// Add data rows
		//	foreach (var item in data)
		//	{
		//		var values = typeof(TicketReport).GetProperties().Select(property => property.GetValue(item)?.ToString() ?? "");
		//		csvContent.AppendLine(string.Join(",", values));
		//	}

		//	return csvContent.ToString();
		//}




		private void CleanDateRange()
		{
			date1 = null;
			date2 = null;
			grid.Reset();
		}
	}
}
