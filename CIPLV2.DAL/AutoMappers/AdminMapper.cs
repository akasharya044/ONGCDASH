using AutoMapper;
using CIPLV2.Models.AdditionalInfo;
using CIPLV2.Models.Admin;

namespace CIPLV2.DAL.AutoMappers
{
	public class AdminMapper : Profile
	{
		public AdminMapper()
		{
			CreateMap<Test, TestDTO>()
			 .ForMember(dest => dest.TestId, src => src.MapFrom(x => x.Id)).ReverseMap();
			CreateMap<AdminUsers, AdminUsersDto>()
			  .ForMember(dest => dest.UserName, src => src.MapFrom(x => x.Name)).ReverseMap();

			CreateMap<AdditionalInformation, AdditionalInformationDTO>()

			.ReverseMap();
			CreateMap<AdditionalInformationHardDisk, AdditionalInformationHardDiskDto>().ReverseMap();

			CreateMap<NoServices, NoServicesDto>().ReverseMap();

			CreateMap<DeviceData, DeviceDataDto>().ReverseMap();
		}
	}
}
