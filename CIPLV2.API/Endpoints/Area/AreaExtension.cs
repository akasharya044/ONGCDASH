using CIPLV2.DAL.DataService;
using CIPLV2.Models.Area;

namespace CIPLV2.API.Endpoints
{ 
    public static partial class AreaExtensions
    {

        public static async Task<IResult> AddArea(List<AreaDTO> data, IDataService dataService)
        {
            var response = await dataService.areadata.AddArea(data);
            return Results.Ok(response);
        }

        public static async Task<IResult> GetArea(IDataService dataService, int categoryId, int SubcategoryId)
        {
            var response = await dataService.areadata.GetArea(categoryId,SubcategoryId);
            return Results.Ok(response);
        }
		public static async Task<IResult> GetAreas(IDataService dataService, int categoryId, int SubcategoryId)
		{
			var response = await dataService.areadata.GetAreas(categoryId, SubcategoryId);
			return Results.Ok(response);
		}
		public static async Task<IResult> GetAreaAll(IDataService dataService)
		{
			var response = await dataService.areadata.GetAreaAll();
			return Results.Ok(response);
		}
	}
}
