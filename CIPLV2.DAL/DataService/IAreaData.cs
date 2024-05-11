using CIPLV2.Models.Admin;
using CIPLV2.Models.Area;

namespace CIPLV2.DAL.DataService
{
    public interface IAreaData
    {
        Task<Response> AddArea(List<AreaDTO> data);
        Task<Response> GetArea (int categoryId,int SubcategoryId);
        Task<Response> GetAreas(int categoryId, int SubcategoryId);
		Task<Response> GetAreaAll();
	}
}
