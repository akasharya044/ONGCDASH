using AutoMapper;
using CIPLV2.Models.Incidents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.DAL.AutoMappers
{
    public class IncidentMapper : Profile
    {
        public IncidentMapper()
        {
            CreateMap<Incident, IncidentDTO>().ReverseMap();
        }
    }
}
