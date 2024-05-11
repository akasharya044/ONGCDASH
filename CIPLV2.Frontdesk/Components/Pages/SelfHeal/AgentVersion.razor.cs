using CIPLV2.Frontdesk.Components.Dto;
using CIPLV2.Frontdesk.Services;
using CIPLV2.Models.Registration;
using Newtonsoft.Json;

namespace CIPLV2.Frontdesk.Components.Pages.SelfHeal
{
	public partial class AgentVersion
	{
		IEnumerable<DeviceDetailsDtos> _systeminfo { get; set; }
		private DeviceDto device { get; set; } = new DeviceDto();
		private System.Threading.Timer? timer;
		bool loading { get; set; } = true;
		protected override async Task OnInitializedAsync()
		{
			if (sessionData.UserData == null || sessionData.UserData.UserName == null)
				NavigationManager.NavigateTo("/");

			await base.OnInitializedAsync();
			await LoadData();
			timer = new System.Threading.Timer(async (object? stateInfo) =>
			{
				device = SessionState.Device;
				//StateHasChanged();
				await InvokeAsync(StateHasChanged);

			}, new System.Threading.AutoResetEvent(false), 1000, 1000); // fire every 2000 milliseconds

		}
		private async Task UpdateDevicedto(DeviceDto input)
		{
			device = new DeviceDto() { RunningDevices = input.RunningDevices, TotalDevices = input.TotalDevices };
			//await Task.Delay(500);
			await InvokeAsync(StateHasChanged);
		}

		private async Task LoadData()
		{
			var output = await ciplapiservice.GetData("/api/devicedetails");
			if (output != null && output.Status == "Success")
			{
				_systeminfo = JsonConvert.DeserializeObject<IEnumerable<DeviceDetailsDtos>>(output.Data.ToString());
				loading = false;
				_systeminfo.ToList().ForEach(s =>
				{
					if (s.properties.IsDeleted == false)
					{
						s.properties.Status = "Running";
					}
					else
					{
						s.properties.Status = "Offline";
					}
				});
			}
		}
	}
}
