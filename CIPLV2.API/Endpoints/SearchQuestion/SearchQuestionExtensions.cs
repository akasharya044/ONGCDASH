using CIPLV2.DAL.DataService;
using CIPLV2.Models.SubCategory;

namespace CIPLV2.API.Endpoints
{
   
    public static partial class SearchQuestionExtensions
    {
        
        public static async Task<IResult> GetAllSearchQuestionList(IDataService dataService)
        {
            var response = await dataService.searchQuestion.GetAllSearchQuestion();
            return Results.Ok(response);
        }
    
      
    }
}
