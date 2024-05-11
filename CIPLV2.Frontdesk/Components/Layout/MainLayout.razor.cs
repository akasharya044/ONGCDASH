namespace CIPLV2.Frontdesk.Components.Layout
{
	public partial class MainLayout
	{
		protected override async Task OnInitializedAsync()
		{
			//if (sessionData.UserData == null || sessionData.UserData.UserName == null)
			//	NavigationManager.NavigateTo("/");

			await base.OnInitializedAsync();
		}
		private async Task Logout()
		{
			sessionData.UserData = null;
			sessionData.UserData.UserName = null;
			NavigationManager.NavigateTo("/");

		}
	}
}
