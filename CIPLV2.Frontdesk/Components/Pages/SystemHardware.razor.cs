using CIPLV2.Models.UserSystemHardware;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Radzen;
using System.Text;

namespace CIPLV2.Frontdesk.Components.Pages
{
    public partial class SystemHardware
    {
        Variant variant = Variant.Filled;

        List<UserSystemHardwareDto> _SystemHardware { get; set; } = new List<UserSystemHardwareDto>();
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
                var output = await ciplapiservice.GetData("api/system/hardwareinfo");
                if (output != null && output.Status == "Success")
                {
                    _SystemHardware = JsonConvert.DeserializeObject<List<UserSystemHardwareDto>>(output.Data.ToString());
                    loading = false;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }


        private async Task DownloadCsvHardware()
        {
            // Generate CSV content based on ticketData
            var csvContent = GenerateCsvContentinstalled(_SystemHardware);

            // Prepare the file content for download
            var data = Encoding.UTF8.GetBytes(csvContent);

            // Initiate the file download using FileSaver
            await JSRuntime.InvokeVoidAsync("saveAsFile", "SytemHardware.csv", data, "text/csv");
            //ticketData = ticketData1;
        }


        private string GenerateCsvContentinstalled(IEnumerable<UserSystemHardwareDto> data)
        {
            // Generate CSV content based on your data structure
            StringBuilder csvContent = new StringBuilder();

            // Add header row
            var headers = typeof(UserSystemHardwareDto).GetProperties().Select(property => property.Name);
            csvContent.AppendLine(string.Join(",", headers));

            // Add data rows
            foreach (var item in data)
            {
                var values = typeof(UserSystemHardwareDto).GetProperties().Select(property => property.GetValue(item)?.ToString() ?? "");
                csvContent.AppendLine(string.Join(",", values));
            }

            return csvContent.ToString();
        }
    }
}
