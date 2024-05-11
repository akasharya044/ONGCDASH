using System.Runtime.CompilerServices;
using System.Text;
using CIPLV2.Models.Category;
using CIPLV2.Models.SubCategory;
using CIPLV2.Models.Tickets;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Radzen;
using Radzen.Blazor;

namespace CIPLV2.Frontdesk.Components.Pages;

public partial class QuestionsLanding
{
    List<SubCategoriesDto> _subcategories { get; set; } = new List<SubCategoriesDto>();
    Variant variant = Variant.Filled;
	IEnumerable<int> pageSizeOptions = new int[] { 10, 20, 30 };
	List<QuestionDto> questiondtolist = new List<QuestionDto>();
	RadzenDataGrid<QuestionDto> grid;


	protected override async Task OnInitializedAsync()
	{
		if (sessionData.UserData == null || sessionData.UserData.UserName == null)
			NavigationManager.NavigateTo("/");

		await base.OnInitializedAsync();
		await GetQuestions();

	}
	private void ShowAddQuestion()
	{
		NavigationManager.NavigateTo("/addquestion");

	}
	private async Task GetQuestions()
	{
		try
		{
			var output = await ciplapiservice.GetData("/api/subcategories/question/answer");
			if (output != null && output.Status == "Success")
			{
				questiondtolist = JsonConvert.DeserializeObject<List<QuestionDto>>(output.Data.ToString());
			}

			StateHasChanged();
		}
		catch (Exception ex) { }
	}
	private async Task DownloadExcel()
	{
		var csvContent = GenerateCsvContentinstalled(questiondtolist);
		var data = Encoding.UTF8.GetBytes(csvContent);
		await JSRuntime.InvokeVoidAsync("saveAsFile", "Question&Answer.csv", data, "text/csv");
	}


	private string GenerateCsvContentinstalled(IEnumerable<QuestionDto> data)
	{
		StringBuilder csvContent = new StringBuilder();
		var headers = typeof(QuestionDto).GetProperties().Select(property => property.Name);
		csvContent.AppendLine(string.Join(",", headers));

		foreach (var item in data)
		{
			var values = typeof(QuestionDto).GetProperties().Select(property => property.GetValue(item)?.ToString() ?? "");
			csvContent.AppendLine(string.Join(",", values));
		}

		return csvContent.ToString();
	}
}
