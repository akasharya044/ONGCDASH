
using CIPLV2.Models.Category;
using CIPLV2.Models.Registration;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using OfficeOpenXml;
using Radzen;
using System.Text;

namespace CIPLV2.Frontdesk.Components.Pages
{
    public partial class Categories
    {
        Variant variant = Variant.Filled;
        bool loading { get; set; } = true;
        List<CategoriesDto> _categories { get; set; } = new List<CategoriesDto>();
        public IBrowserFile? SelectedFile { get; private set; }

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

            var output = await ciplapiservice.GetData("/api/categories");
            if (output != null && output.Status == "Success")
            {
                _categories = JsonConvert.DeserializeObject<List<CategoriesDto>>(output.Data.ToString());
                loading = false;
            }
        }

        private async Task DownloadCsv()
        {
            // Generate CSV content based on ticketData
            var csvContent = GenerateCsvContent(_categories);

            // Prepare the file content for download
            var data = Encoding.UTF8.GetBytes(csvContent);

            // Initiate the file download using FileSaver
            await JSRuntime.InvokeVoidAsync("saveAsFile", "CategoriesInfoReport.csv", data, "text/csv");
            //ticketData = ticketData1;
        }


        private string GenerateCsvContent(IEnumerable<CategoriesDto> data)
        {
            // Generate CSV content based on your data structure
            StringBuilder csvContent = new StringBuilder();

            // Add header row
            var headers = typeof(CategoriesDto).GetProperties().Select(property => property.Name);
            csvContent.AppendLine(string.Join(",", headers));

            // Add data rows
            foreach (var item in data)
            {
                var values = typeof(CategoriesDto).GetProperties().Select(property => property.GetValue(item)?.ToString() ?? "");
                csvContent.AppendLine(string.Join(",", values));
            }

            return csvContent.ToString();
        }

        private async Task OpenFilePicker()
        {
            // Trigger file input click event
            await JSRuntime.InvokeVoidAsync("openFilePicker", "upload");
        }
        public async Task Validate(List<CategoriesDto> catdata)
        {


            var response = await ciplapiservice.PostData(catdata, "/api/categories");
            if (response != null)
            {
                _categories = JsonConvert.DeserializeObject<List<CategoriesDto>>(response.Data.ToString());
                dialogService.Alert("Upload Successfully");

            }

        }

        public async Task OnChange(InputFileChangeEventArgs e)
        {
            SelectedFile = e.GetMultipleFiles().FirstOrDefault();
            var excelData = await CategoriesUploadFile(SelectedFile);
            if (excelData != null)
            {
                await Validate(excelData);
            }


        }


        public async Task<List<CategoriesDto>> CategoriesUploadFile(IBrowserFile file)
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
                            var rowData = new CategoriesDto();
                            rowData.MfCatId = Convert.ToInt64(worksheet.Cells[row, 1].Value);
                            rowData.entitytype = worksheet.Cells[row, 2].Value.ToString();
                            rowData.displaylabel = worksheet.Cells[row, 3].Value.ToString();
                            rowData.phaseid = worksheet.Cells[row, 4].Value.ToString();
                            rowData.Category_c = worksheet.Cells[row, 5].Value.ToString();
                            rowData.impmcategory_c = (bool)(worksheet.Cells[row, 6].Value);
                            _categories.Add(rowData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }
            return _categories;
        }

        public async Task DownloadExcel(ExcelPackage package)
        {

        }
    }
}
