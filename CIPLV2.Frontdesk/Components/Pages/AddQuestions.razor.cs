using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using CIPLV2.Models.Area;
using CIPLV2.Models.Category;
using CIPLV2.Models.SubCategory;
using Newtonsoft.Json;
using Radzen;

namespace CIPLV2.Frontdesk.Components.Pages;

public partial class AddQuestions
{
	QuestionDto questiondto = new QuestionDto();
	List<SubCategoriesDto> _subCategories = new List<SubCategoriesDto>();
	List<CategoriesDto> _categories { get; set; } = new List<CategoriesDto>();
	List<AreaDTO> _arealist { get; set; } = new List<AreaDTO>();

	protected override async Task OnInitializedAsync()
	{
		if (sessionData.UserData == null || sessionData.UserData.UserName == null)
			NavigationManager.NavigateTo("/");

		await base.OnInitializedAsync();

		await CategoriesData();


	}
	private async Task CategoriesData()
	{

		var output = await ciplapiservice.GetData("/api/categories");
		if (output != null && output.Status == "Success")
		{
			_categories = JsonConvert.DeserializeObject<List<CategoriesDto>>(output.Data.ToString());
		}
	}
	private async Task SubCategoriesData(long Id)
	{

		var output = await ciplapiservice.GetData("/api/subcategories/{categoriesid}?categoryId=" + Id);
		if (output != null && output.Status == "Success")
		{
			_subCategories = JsonConvert.DeserializeObject<List<SubCategoriesDto>>(output.Data.ToString());
		}
	}
	private async Task AreaData(long CatId, long SubCatId)
	{

		var output = await ciplapiservice.GetData("api/area/areas?categoryId=" + CatId + "&SubcategoryId=" + SubCatId);
		if (output != null && output.Status == "Success")
		{
			_arealist = JsonConvert.DeserializeObject<List<AreaDTO>>(output.Data.ToString());
		}
	}
	private async Task UpsertData()
	{
		try
		{
			if (questiondto.MfAreaId == 0 || questiondto.categoryId == 0 || questiondto.subCategoryId == 0)
			{
				dialogService.Alert("Please select DropDown");

			}
			else
			{
				var response = await ciplapiservice.PostData(questiondto, "api/subcategories/question/answer");
				if (response != null && response.Status == "Success")
				{
					dialogService.Alert("Data is saved");
				}
				else
				{
					dialogService.Alert("Data is not saved");
				}
			}

		}
		catch (Exception ex)
		{
			dialogService.Close();

		}
		NavigationManager.NavigateTo("/questionslanding");

	}
	private void BackList()
	{
		NavigationManager.NavigateTo("/questionslanding");
	}
	async Task OnInvalidSubmit(FormInvalidSubmitEventArgs args)
	{
		var errormessage = args.Errors.First();
		string[] words = errormessage.Split(' ');
		for (int i = 0; i < words.Length; i++)
		{
			words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
		}
		string camelCase = string.Concat(words);
		camelCase = char.ToLower(camelCase[0]) + camelCase.Substring(1);
		System.Text.RegularExpressions.Match match = Regex.Match(camelCase, "([a-zA-Z0-9]+)IsRequired.");
		if (match.Success)
		{
			errormessage = match.Groups[1].Value;
		}
		string firstError = $"{char.ToUpper(errormessage[0])}{errormessage.Substring(1)}";
		Console.WriteLine(firstError);
	}

}
