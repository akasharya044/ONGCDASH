namespace CIPLV2.API.Endpoints
{
    public static partial class SearchQuestionExtensions
    {
        public static RouteGroupBuilder MapSearchQuestionEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("", GetAllSearchQuestionList);
           
            return group;
        }
    }
}
