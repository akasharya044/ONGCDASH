using AutoMapper;
using CIPLV2.DAL.DataService;
using CIPLV2.Models.Admin;
using CIPLV2.Models.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.DAL.AutoMappers
{
    public class TicketMappers : Profile
    {
        public TicketMappers()
        {
            CreateMap<TicketRecord, RaiseTicketDto>().ReverseMap();
            CreateMap<TicketRecord, RaiseMFTicketDTO>().ReverseMap();
            CreateMap<StarRating, StarRatingDTO>().ReverseMap();
            CreateMap<RaiseAgentTicketDTO, RaiseMFTicketDTO>().ReverseMap();
            CreateMap<AgentEntity, MFEntity>().ReverseMap();
            CreateMap<AgentProperty, MFProperty>().ReverseMap();
            
        }
    }
}
