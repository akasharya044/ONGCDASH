using CIPLV2.DAL.DataService;
using CIPLV2.Models.SubCategory;

namespace CIPLV2.API.Endpoints
{
	public static partial class SubCategoriesExtensions
	{
		public static async Task<IResult> SubCategoriesList(IDataService dataService,int categoryId)
		{
			var response = await dataService.subCategoriesData.GetSubCategories(categoryId);
			return Results.Ok(response);
		}
		public static async Task<IResult> SubGetAllCategoriesList(IDataService dataService)
		{
			var response = await dataService.subCategoriesData.GetAllSubCategories();
			return Results.Ok(response);
		}
		public static async Task<IResult> QuestionsList(IDataService dataService)
		{
			var response = await dataService.subCategoriesData.GetQuestion();
			return Results.Ok(response);
		}
        public static async Task<IResult> QuestionsByText(IDataService dataService,string text)
        {
            var response = await dataService.subCategoriesData.GetQuestionByKeyword(text);
            return Results.Ok(response);
        }
        public static async Task<IResult> AnswersList(IDataService dataService, int questionId)
		{
			var response = await dataService.subCategoriesData.GetAnswers(questionId);
			return Results.Ok(response);
		}
		public static async Task<IResult> AddSubCategories(List<SubCategoriesDto> data, IDataService dataService)
		{
			var response = await dataService.subCategoriesData.AddSubCategories(data);
			return Results.Ok(response);
		}
		public static async Task<IResult> AddQuestions(List<QuestionsDto> data, IDataService dataService)
		{
			var response = await dataService.subCategoriesData.AddQuestions(data);
			return Results.Ok(response);
		}
		public static async Task<IResult> AddAnswers(List<AnswersDto> data, IDataService dataService)
		{
			var response = await dataService.subCategoriesData.AddAnswers(data);
			return Results.Ok(response);
		}
		public static async Task<IResult> AddQuestionAnswer(QuestionDto data, IDataService dataService)
		{
			var response = await dataService.subCategoriesData.AddQuestionAnswer(data);
			return Results.Ok(response);
		}
		public static async Task<IResult> GetQuestionAnswerList(IDataService dataService)
		{
			var response = await dataService.subCategoriesData.GetQuestionAnswers();
			return Results.Ok(response);
		}


	}
}
