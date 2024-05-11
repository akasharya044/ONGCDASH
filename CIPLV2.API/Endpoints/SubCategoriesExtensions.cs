namespace CIPLV2.API.Endpoints
{
	public static partial class SubCategoriesExtensions
	{
		public static RouteGroupBuilder MapSubCategoryEndpoint(this RouteGroupBuilder group)
		{
			group.MapGet("/{categoriesid}", SubCategoriesList);
			group.MapPost("", AddSubCategories);
			group.MapGet("", SubGetAllCategoriesList);
			group.MapGet("/questions", QuestionsList);
			group.MapGet("/questions/text", QuestionsByText);
			group.MapPost("/questions", AddQuestions);
			group.MapGet("/{questionId}/answers", AnswersList);
			group.MapPost("/answers", AddAnswers);
			group.MapPost("/question/answer", AddQuestionAnswer);
			group.MapGet("/question/answer", GetQuestionAnswerList);
			return group;
		}
	}
}
