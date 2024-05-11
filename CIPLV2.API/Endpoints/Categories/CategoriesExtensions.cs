using CIPLV2.DAL.DataService;
using CIPLV2.Models.Category;

namespace CIPLV2.API.Endpoints
{
	public static partial class CategoriesExtensions
	{
		public static async Task<IResult> CategoriesList(IDataService dataService)
		{
			var response = await dataService.categoriesData.GetCategories();
			return Results.Ok(response);
		}
		public static async Task<IResult> AddCategories(List<CategoriesDto> data, IDataService dataService)
		{
			var response = await dataService.categoriesData.AddCategories(data);
			return Results.Ok(response);
		}
	}
}
