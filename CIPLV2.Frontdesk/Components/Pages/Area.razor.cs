using CIPLV2.Models.Area;
using CIPLV2.Models.Category;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Text;

namespace CIPLV2.Frontdesk.Components.Pages
{
    public partial class Area
    {
        List<AreaDTO> _area { get; set; } = new List<AreaDTO>();
        public IBrowserFile? SelectedFile { get; private set; }

        IEnumerable<int> pageSizeOptions = new int[] { 10, 20, 30 };
        bool loading { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            if (sessionData.UserData == null || sessionData.UserData.UserName == null)
                NavigationManager.NavigateTo("/");

            await base.OnInitializedAsync();

            await Task.Run(LoadData);


        }
        private async Task LoadData()
        {
            System.Threading.Thread.Sleep(2000);
            var output = await ciplapiservice.GetData("/api/area/getarea");
            if (output != null && output.Status == "Success")
            {
                _area = JsonConvert.DeserializeObject<List<AreaDTO>>(output.Data.ToString());
                loading = false;
            }
        }


        public async Task Validate(List<AreaDTO> catdata)
        {


            var response = await ciplapiservice.PostData(catdata, "/api/area");
            if (response != null)
            {
                _area = JsonConvert.DeserializeObject<List<AreaDTO>>(response.Data.ToString());
                dialogService.Alert("Upload Successfully");

            }

        }

        public async Task OnChange(InputFileChangeEventArgs e)
        {
            SelectedFile = e.GetMultipleFiles().FirstOrDefault();
            var excelData = await AreasUploadFile(SelectedFile);
            if (excelData != null)
            {
                await Validate(excelData);
            }


        }


        public async Task<List<AreaDTO>> AreasUploadFile(IBrowserFile file)
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

                        for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                        {
                            var rowData = new AreaDTO();
                            rowData.MfAreaId = Convert.ToInt64(worksheet.Cells[row, 1].Value);
                            rowData.entitytype = worksheet.Cells[row, 2].Value.ToString();
                            rowData.category_c = Convert.ToInt32(worksheet.Cells[row, 3].Value);
                            rowData.Category_c_DisplayLabel = worksheet.Cells[row, 4].Value.ToString();
                            rowData.subcategory_c = Convert.ToInt32(worksheet.Cells[row, 5].Value);
                            rowData.Subcategory_c_DisplayLabel = worksheet.Cells[row, 6].Value.ToString();
                            rowData.displaylabel = worksheet.Cells[row, 7].Value.ToString();
                            rowData.phaseid = worksheet.Cells[row, 8].Value.ToString();
                            rowData.priority_c = Convert.ToInt32(worksheet.Cells[row, 9].Value);
                            rowData.impmcategory_c = (bool)(worksheet.Cells[row, 10].Value);
                            _area.Add(rowData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }
            return _area;
        }

        private async Task DownloadCsv()
        {
            // Generate CSV content based on ticketData
            var csvContent = GenerateCsvContent(_area);

            // Prepare the file content for download
            var data = Encoding.UTF8.GetBytes(csvContent);

            // Initiate the file download using FileSaver
            await JSRuntime.InvokeVoidAsync("saveAsFile", "AreaInfoReport.csv", data, "text/csv");
            //ticketData = ticketData1;
        }


        private string GenerateCsvContent(IEnumerable<AreaDTO> data)
        {
            // Generate CSV content based on your data structure
            StringBuilder csvContent = new StringBuilder();

            // Add header row
            var headers = typeof(AreaDTO).GetProperties().Select(property => property.Name);
            csvContent.AppendLine(string.Join(",", headers));

            // Add data rows
            foreach (var item in data)
            {
                var values = typeof(AreaDTO).GetProperties().Select(property => property.GetValue(item)?.ToString() ?? "");
                csvContent.AppendLine(string.Join(",", values));
            }

            return csvContent.ToString();
        }





    }
}
