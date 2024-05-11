using AutoMapper;
using CIPLV2.Models.Admin;
using CIPLV2.Models.Area;

namespace CIPLV2.DAL.AutoMappers
{
	public class AreaMappers : Profile
	{
		public AreaMappers()
		{
			CreateMap<Areas, AreaDTO>().ReverseMap();
		}
	}
}
