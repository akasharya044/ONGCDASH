using AutoMapper;
using CIPLV2.Models.EventHistories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.DAL.AutoMappers
{
    public class EventHistoriesMapper :Profile
    {
        public EventHistoriesMapper()
        {
            CreateMap<EventHistory, EventHistoryDto>().ReverseMap();
        }
    }
}
