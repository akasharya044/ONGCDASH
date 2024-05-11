using CIPLV2.Models.Admin;
using CIPLV2.Models.Incidents;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.DAL.DataService
{
    public interface IIncidentData
    {
        Task<Response> GetIncidentById(int Id);

        Task<Response> GetAllIncident(SieveModel model);

        Task<Response> Upsert(IncidentDTO incidentDTO);





    }
}
