using AutoMapper;
using CIPLV2.Models.SubCategory;

namespace CIPLV2.DAL.AutoMappers
{
	public class SubCategoriesMapper : Profile
	{
		public SubCategoriesMapper()
		{
			CreateMap<SubCategories,SubCategoriesDto>()
				.ForMember(dest => dest.category_c, src => src.MapFrom(x => x.category_c)).ReverseMap();
			CreateMap<Questions, QuestionsDto>().ReverseMap();
			CreateMap<Questions, QuestionDto>().ReverseMap();
			CreateMap<Answers, AnswersDto>().ReverseMap();
		}
	}
}
