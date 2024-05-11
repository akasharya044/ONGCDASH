namespace CIPLV2.API.Endpoints
{
	public static partial class CategoriesExtensions
	{
		public static RouteGroupBuilder MapCategoryEndpoint(this RouteGroupBuilder group)
		{
			group.MapGet("", CategoriesList);
			group.MapPost("", AddCategories);
			return group;
		}

	}
}
