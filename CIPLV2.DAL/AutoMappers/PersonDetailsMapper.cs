using AutoMapper;
using CIPLV2.Models.PersonDetail;

namespace CIPLV2.DAL.AutoMappers
{
	public class PersonDetailsMapper : Profile
	{
		public PersonDetailsMapper()
		{
			CreateMap<PersonDetails, PersonDetailsDto>()
			 //.ForMember(dest => dest.TestId, src => src.MapFrom(x => x.Id))
			 .ReverseMap();
			//CreateMap<Properties, PersonDetailsDto>()
			//	.ForPath(dest => dest.properties.FirstName, src => src.MapFrom(x => x.FirstName))
			//	.ForPath(dest => dest.properties.Id, src => src.MapFrom(x => x.Id))
			//	.ForPath(dest => dest.properties.IsDeleted, src => src.MapFrom(x => x.IsDeleted))
			//	.ForPath(dest => dest.properties.Email, src => src.MapFrom(x => x.Email))
			//	.ForPath(dest => dest.properties.LastName, src => src.MapFrom(x => x.LastName))
			//	.ForPath(dest => dest.properties.EmployeeNumber, src => src.MapFrom(x => x.EmployeeNumber))
			//	.ForPath(dest => dest.properties.Name, src => src.MapFrom(x => x.Name))
			//	.ForPath(dest => dest.properties.LastUpdateTime, src => src.MapFrom(x => x.LastUpdateTime))
			//	.ForPath(dest => dest.properties.Upn, src => src.MapFrom(x => x.Upn)).ReverseMap();
			
			CreateMap<PersonDetails, PersonDetailsDto>()
				.ForPath(dest => dest.properties.FirstName, src => src.MapFrom(x => x.FirstName))
				.ForPath(dest => dest.properties.Id, src => src.MapFrom(x => x.Id))
				.ForPath(dest => dest.properties.IsDeleted, src => src.MapFrom(x => x.IsDeleted))
				.ForPath(dest => dest.properties.Email, src => src.MapFrom(x => x.Email))
				.ForPath(dest => dest.properties.LastName, src => src.MapFrom(x => x.LastName))
				.ForPath(dest => dest.properties.EmployeeNumber, src => src.MapFrom(x => x.EmployeeNumber))
				.ForPath(dest => dest.properties.Name, src => src.MapFrom(x => x.Name))
				.ForPath(dest => dest.properties.LastUpdateTime, src => src.MapFrom(x => x.LastUpdateTime))
				.ForPath(dest => dest.properties.MfpersonId, src => src.MapFrom(x => x.MfpersonId))
				.ForPath(dest => dest.properties.Upn, src => src.MapFrom(x => x.Upn)).ReverseMap();
		}
	}
}
