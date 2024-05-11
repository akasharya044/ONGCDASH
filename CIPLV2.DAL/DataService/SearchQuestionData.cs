using AutoMapper;
using CIPLV2.DAL.Unitofworks;
using CIPLV2.Models.Admin;

namespace CIPLV2.DAL.DataService
{
    public  class SearchQuestionData : ISearchQuestionData  
    {
        readonly IUnitOfWorks _uow;
        readonly IMapper _mapper;

        public SearchQuestionData(IUnitOfWorks uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<Response> GetAllSearchQuestion()
        {
            Response response = new Response();
            try
            {
                var data = await _uow.searchQuestion.GetAllAsync();
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
    }
}
