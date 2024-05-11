using AutoMapper;
using CIPLV2.Models.Registration;

namespace CIPLV2.DAL.AutoMappers
{
	public class MachineRegistrationMapper : Profile
	{
		public MachineRegistrationMapper()
		{
			CreateMap<MachineRegistration, MachineRegistrationDto>().ReverseMap();
		}

	}
}
