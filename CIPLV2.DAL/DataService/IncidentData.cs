using AutoMapper;
using CIPLV2.DAL.Unitofworks;
using CIPLV2.Models.Admin;
using CIPLV2.Models.Incidents;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPLV2.DAL.DataService
{
    public class IncidentData : IIncidentData
    {
        readonly IUnitOfWorks _uow;
        readonly ISieveProcessor _sieveProcessor;

        readonly IMapper _mapper;

        public IncidentData(IUnitOfWorks uow, ISieveProcessor sieveProcessor, IMapper mapper)
        {
            _uow = uow;
            _sieveProcessor = sieveProcessor;
            _mapper = mapper;
        }
        public async Task<Response> GetIncidentById(int Id)
        {
            Response response = new Response();
            try
            {
                var data = await _uow.incident.GetAllAsync(x => x.TicketId == Id);
                if (data != null && data.Count() > 0)
                {
                    response.Status = "Success";
                    response.Message = "Data Fetch Successfully";
                    response.Data = data;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "No Record Found!!";
                    response.Data = data;
                }
            }
            catch (Exception ex)
            {
                response.Status = "Failed";
                var errormessage = await _uow.AddException(ex);
                response.Message = errormessage;
            }
            return response;
        }
        public async Task<Response> GetAllIncident(SieveModel model)
        {
            Response response = new Response();
            try
            {
                var data = await _uow.incident.GetAllAsync();
                var result = _sieveProcessor.Apply(model, data.AsQueryable());
                if (result != null && result.Count() > 0)
                {
                    response.Status = "Success";
                    response.Message = "Data Fetch Successfully";
                    response.Data = result;
                }
                else
                {
                    response.Status = "Success";
                    response.Message = "No Record Found!!";
                    response.Data = data;
                }
            }
            catch (Exception ex)
            {
                response.Status = "Failed";
                var errormessage = await _uow.AddException(ex);
                response.Message = errormessage;
            }
            return response;

        }
        public async Task<Response> Upsert(IncidentDTO incidentDTO)
        {
            Response response = new Response();
            try
            {
                var data = _mapper.Map<Incident>(incidentDTO);

                if (data.TicketId == 0)
                {
                    _uow.incident.Add(data);

                }
                else
                {
                    _uow.incident.Update(data);


                }

                await _uow.SaveAsync();
                response.Status = "Success";
                response.Message = "Data Saved";
                response.Data = null;
            }
            catch (Exception ex)
            {
                response.Status = "Failed";
                var errormessage = await _uow.AddException(ex);
                response.Message = errormessage;
            }
            return response;
        }
     
    }
}
