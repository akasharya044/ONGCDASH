
using CIPLV2.Models.Admin;
using CIPLV2.Models.Category;
using CIPLV2.Models.SubCategory;

namespace CIPLV2.DAL.DataService
{
	public interface ICategoriesData
	{
		Task<Response> GetCategories();
		Task<Response> AddCategories(List<CategoriesDto> data);
	}
}
