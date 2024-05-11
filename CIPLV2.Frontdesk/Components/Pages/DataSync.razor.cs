using CIPLV2.Models.EventHistories;
using CIPLV2.Models.PersonDetail;
using Newtonsoft.Json;

namespace CIPLV2.Frontdesk.Components.Pages
{
	public partial class DataSync
	{
		List<DeviceDetailDto> devices = new List<DeviceDetailDto>();
		List<PersonDetails> users = new List<PersonDetails>();
		bool loading { get; set; } = true;
		protected override async Task OnInitializedAsync()
		{
			if (sessionData.UserData == null || sessionData.UserData.UserName == null)
				NavigationManager.NavigateTo("/");

			await base.OnInitializedAsync();
		}

		private async Task SyncUserData()
		{
			try
			{
				var output = await ciplapiservice.GetData("/api/tickets/person");
				if (output != null && output.Status == "Success")
				{
					users = JsonConvert.DeserializeObject<List<PersonDetails>>(output.Data.ToString());

					dialogService.Alert("Data Sync Successfully");
					StateHasChanged();
				}
				else
				{

					dialogService.Alert("Data Sync Failed");

				}
			}
			catch (Exception ex)
			{
				dialogService.Alert("Data Sync Failed");

				Console.WriteLine($"Error: {ex.Message}");
			}
		}
		private async Task SyncDeviceData()
		{
			try
			{
				var output = await ciplapiservice.GetData("/api/tickets/device");
				if (output != null && output.Status == "Success")
				{
					devices = JsonConvert.DeserializeObject<List<DeviceDetailDto>>(output.Data.ToString());

					dialogService.Alert("Data Sync Successfully");
					StateHasChanged();
				}
				else
				{

					dialogService.Alert("Data Sync Failed");
				}
			}
			catch (Exception ex)
			{
				dialogService.Alert("Data Sync Failed");

				Console.WriteLine($"Error: {ex.Message}");
			}
		}

	}
}
