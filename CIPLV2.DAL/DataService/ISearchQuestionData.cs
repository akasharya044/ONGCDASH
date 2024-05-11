using CIPLV2.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.DAL.DataService
{
    public interface ISearchQuestionData
    {
        Task<Response> GetAllSearchQuestion();
    }
}
