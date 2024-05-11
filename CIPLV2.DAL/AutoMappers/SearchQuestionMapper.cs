using AutoMapper;
using CIPLV2.Models.Incidents;
using CIPLV2.Models.SearchQuestion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.DAL.AutoMappers
{
   
    public class SearchQuestionMapper : Profile
    {
        public SearchQuestionMapper()
        {
            CreateMap<SearchQuestion, SearchQuestionDto>().ReverseMap();
        }
    }
}
