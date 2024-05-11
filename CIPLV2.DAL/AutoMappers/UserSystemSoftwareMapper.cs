using AutoMapper;
using CIPLV2.DAL.DataService;
using CIPLV2.Models.UserSystemHardware;
using CIPLV2.Models.UserSystemSoftware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.DAL.AutoMappers
{
    public class UserSystemSoftwareMapper : Profile
    {
        public UserSystemSoftwareMapper() 
        {
            CreateMap<UserSystemSoftware,UserSystemSoftwareDto>().ReverseMap();
            CreateMap<UserSystemHardware, UserSystemHardwareDto>().ReverseMap();
        }

    }
}
