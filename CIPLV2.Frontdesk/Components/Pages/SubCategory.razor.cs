using CIPLV2.Models.SubCategory;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using OfficeOpenXml;
using Radzen;
using System.Text;

namespace CIPLV2.Frontdesk.Components.Pages
{
    public partial class SubCategory
    {
        Variant variant = Variant.Filled;
        public IBrowserFile? SelectedFile { get; private set; }
        List<SubCategoriesDto> _subcategories { get; set; } = new List<SubCategoriesDto>();
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
            var output = await ciplapiservice.GetData("/api/subcategories/");
            if (output != null && output.Status == "Success")
            {
                _subcategories = JsonConvert.DeserializeObject<List<SubCategoriesDto>>(output.Data.ToString());
                loading = false;
            }
        }
        private async Task OpenFilePicker()
        {
            // Trigger file input click event
            await JSRuntime.InvokeVoidAsync("openFilePicker", "upload");
        }
        public async Task Validate(List<SubCategoriesDto> catdata)
        {


            var response = await ciplapiservice.PostData(catdata, "/api/subcategories");
            if (response != null)
            {
                _subcategories = JsonConvert.DeserializeObject<List<SubCategoriesDto>>(response.Data.ToString());
                dialogService.Alert("Upload Successfully");

            }

        }

        public async Task OnChange(InputFileChangeEventArgs e)
        {
            SelectedFile = e.GetMultipleFiles().FirstOrDefault();
            var excelData = await SubCategoriesUploadFile(SelectedFile);
            if (excelData != null)
            {
                await Validate(excelData);
            }


        }


        public async Task<List<SubCategoriesDto>> SubCategoriesUploadFile(IBrowserFile file)
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
                            var rowData = new SubCategoriesDto();
                            rowData.MfSubCatId = Convert.ToInt64(worksheet.Cells[row, 1].Value);
                            rowData.entitytype = worksheet.Cells[row, 2].Value.ToString();
                            rowData.displaylabel = worksheet.Cells[row, 3].Value.ToString();
                            rowData.category_c = Convert.ToInt64(worksheet.Cells[row, 4].Value);
                            rowData.Category_c_DisplayLabel = worksheet.Cells[row, 5].Value.ToString();
                            rowData.phaseid = worksheet.Cells[row, 6].Value.ToString();
                            rowData.impmcategory_c = (bool)(worksheet.Cells[row, 7].Value);
                            _subcategories.Add(rowData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }
            return _subcategories;
        }



        private async Task DownloadCsv()
        {
            // Generate CSV content based on ticketData
            var csvContent = GenerateCsvContent(_subcategories);

            // Prepare the file content for download
            var data = Encoding.UTF8.GetBytes(csvContent);

            // Initiate the file download using FileSaver
            await JSRuntime.InvokeVoidAsync("saveAsFile", "SubCategoriesInfoReport.csv", data, "text/csv");
            //ticketData = ticketData1;
        }


        private string GenerateCsvContent(IEnumerable<SubCategoriesDto> data)
        {
            // Generate CSV content based on your data structure
            StringBuilder csvContent = new StringBuilder();

            // Add header row
            var headers = typeof(SubCategoriesDto).GetProperties().Select(property => property.Name);
            csvContent.AppendLine(string.Join(",", headers));

            // Add data rows
            foreach (var item in data)
            {
                var values = typeof(SubCategoriesDto).GetProperties().Select(property => property.GetValue(item)?.ToString() ?? "");
                csvContent.AppendLine(string.Join(",", values));
            }

            return csvContent.ToString();
        }
    }
}
