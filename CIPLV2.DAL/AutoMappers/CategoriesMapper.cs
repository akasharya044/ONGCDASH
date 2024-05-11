using AutoMapper;
using CIPLV2.Models.Category;

namespace CIPLV2.DAL.AutoMappers
{
	public class CategoriesMapper : Profile
	{
		public CategoriesMapper()
		{
			CreateMap<Categories,CategoriesDto>().ReverseMap();
		}
	}
}
