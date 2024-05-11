
using AutoMapper;
using CIPLV2.DAL.Unitofworks;
using CIPLV2.Models.Admin;
using CIPLV2.Models.SearchQuestion;
using CIPLV2.Models.SubCategory;

namespace CIPLV2.DAL.DataService
{
	public class SubCategoriesData : ISubCategoriesData
	{
		readonly IUnitOfWorks _uow;
		readonly IMapper _mapper;

		public SubCategoriesData(IUnitOfWorks uow, IMapper mapper)
		{
			_uow = uow;
			_mapper = mapper;
		}
		public async Task<Response> GetSubCategories(int categoryId)
		{
			Response response = new Response();
			try
			{
				var data = await _uow.subCategories.GetAllAsync(x => x.category_c == categoryId);
				var dtodata = _mapper.Map<List<SubCategoriesDto>>(data);
				if (data != null && data.Count() > 0)
				{
					response.Status = "Success";
					response.Message = "Data Fetch Successfully";
					response.Data = dtodata;
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
		public async Task<Response> AddSubCategories(List<SubCategoriesDto> data)
		{
			Response response = new Response();
			try
			{
				var mappeddata = _mapper.Map<List<SubCategories>>(data);
				var newdata = mappeddata.Where(x => x.Id == 0).ToList();
				var existingdata = mappeddata.Where(x => x.Id > 0).ToList();
				if (newdata != null && newdata.Count() > 0)
				{
					_uow.subCategories.AddRange(newdata);
				}
				if (existingdata != null && existingdata.Count() > 0)
				{
					_uow.subCategories.UpdateRange(existingdata);
				}
				
				await _uow.SaveAsync();
				var retdata = _mapper.Map<List<SubCategoriesDto>>(mappeddata);
				response.Status = "Success";
				response.Message = "Data Saved";
				response.Data = retdata;
			}
			catch (Exception ex)
			{
				response.Status = "Failed";
				var errormessage = await _uow.AddException(ex);
				response.Message = errormessage;

			}
			return response;
		}
		public async Task<Response> GetAllSubCategories()
		{
			Response response = new Response();
			try
			{
				var data = await _uow.subCategories.GetAllAsync();
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
		public async Task<Response> GetQuestion()
		{
			Response response = new Response();
			try
			{
				var data = await _uow.questions.GetAllAsync();
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
		public async Task<Response> AddQuestions(List<QuestionsDto> data)
		{
			Response response = new Response();
			try
			{
				var mappeddata = _mapper.Map<List<Questions>>(data);
				var newdata = mappeddata.Where(x => x.Id == 0).ToList();
				var existingdata = mappeddata.Where(x => x.Id > 0).ToList();
				if (newdata != null && newdata.Count() > 0)
				{
					_uow.questions.AddRange(newdata);
				}
				if (existingdata != null && existingdata.Count() > 0)
				{
					_uow.questions.UpdateRange(existingdata);
				}
				await _uow.SaveAsync();
				response.Status = "Success";
				response.Message = "Data Saved";
				response.Data = mappeddata;
			}
			catch (Exception ex)
			{
				response.Status = "Failed";
				var errormessage = await _uow.AddException(ex);
				response.Message = errormessage;

			}
			return response;
		}
        public async Task<Response> GetQuestionByKeyword(string text)
        {
            Response response = new Response();
            try
            {
				SearchQuestion  searcque = new SearchQuestion();
				searcque.SearchText = text;
				searcque.Count += 1;
				var searchques = _uow.searchQuestion.Add(searcque);
				await _uow.SaveAsync();
				var data = await _uow.questions.GetAllAsync(x => x.Question.Contains(text));
				var dtodata = _mapper.Map<List<QuestionsDto>>(data);
				foreach (var item in dtodata)
				{
					if (item.MfAreaId > 0)
					{
						var areadata = await _uow.area.GetFirstOrDefaultAsync(x => x.MfAreaId == item.MfAreaId);
						item.categoryId = areadata!.category_c;
						item.subCategoryId = areadata!.subcategory_c;
					}
				}
				if (data != null && data.Count() > 0)
				{
					response.Status = "Success";
					response.Message = "Data Fetch Successfully";
					response.Data = dtodata;
				}
				else
				{
					response.Status = "Success";
					response.Message = "No Record Found!!";
					response.Data = dtodata;
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

        public async Task<Response> GetAnswers(int questionId)
		{
			Response response = new Response();
			try
			{
				var data = await _uow.answers.GetAllAsync(x => x.QuestionId == questionId);
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
		public async Task<Response> AddAnswers(List<AnswersDto> data)
		{
			Response response = new Response();
			try
			{
				var mappeddata = _mapper.Map<List<Answers>>(data);
				var newdata = mappeddata.Where(x => x.Id == 0).ToList();
				var existingdata = mappeddata.Where(x => x.Id > 0).ToList();
				if (newdata != null && newdata.Count() > 0)
				{
					_uow.answers.AddRange(newdata);
				}
				if (existingdata != null && existingdata.Count() > 0)
				{
					_uow.answers.UpdateRange(existingdata);
				}
				await _uow.SaveAsync();				
				response.Status = "Success";
				response.Message = "Data Saved";
				response.Data = mappeddata;
			}
			catch (Exception ex)
			{
				response.Status = "Failed";
				var errormessage = await _uow.AddException(ex);
				response.Message = errormessage;

			}
			return response;
		}

		public async Task<Response> AddQuestionAnswer(QuestionDto data)
		{
			Response response = new Response();
			try
			{
				var mappeddata = _mapper.Map<Questions>(data);
				var quedata = await _uow.questions.AddAsync(mappeddata);
				await _uow.SaveAsync();

				Answers ans = new Answers();
				ans.QuestionId = quedata.Id;
				ans.Answer = data.Answer;
				var ansdata = await _uow.answers.AddAsync(ans);

				
				await _uow.SaveAsync();
				response.Status = "Success";
				response.Message = "Data Saved";
				response.Data = mappeddata;
			}
			catch (Exception ex)
			{
				response.Status = "Failed";
				var errormessage = await _uow.AddException(ex);
				response.Message = errormessage;

			}
			return response;
		}


		public async Task<Response> GetQuestionAnswers()
		{
			Response response = new Response();
			List<QuestionDto> queansdata = new List<QuestionDto>();

			try
			{
				var quedata = await _uow.questions.GetAllAsync();
				var ansdata = await _uow.answers.GetAllAsync();

				if (quedata != null && quedata.Count() > 0)
				{
					foreach(var que in quedata)
					{
						QuestionDto ques = new QuestionDto();
						var answer = ansdata!.Where(a=>a.QuestionId == que.Id).FirstOrDefault();
						ques.Question = que.Question;
						ques.Id = que.Id;
						ques.MfAreaId = que.MfAreaId;
						ques.Answer = answer!!=null?answer.Answer:"No Answer available";
						if (que.MfAreaId > 0)
						{
							var areadata = await _uow.area.GetFirstOrDefaultAsync(x => x.MfAreaId == que.MfAreaId);
							if (areadata != null)
							{
								ques.Area = areadata.displaylabel;
								ques.categoryId = areadata!.category_c;
								ques.subCategoryId = areadata!.subcategory_c;
								ques.category = areadata.Category_c_DisplayLabel;
								ques.Subcategory = areadata.Subcategory_c_DisplayLabel;
							}
						}
						queansdata.Add(ques);
					}
					//quedata.ToList().ForEach(x =>
					//{

					//});
					response.Status = "Success";
					response.Message = "Data Fetch Successfully";
					response.Data = queansdata;
				}
				else
				{
					response.Status = "Success";
					response.Message = "No Record Found!!";
					response.Data = queansdata;
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
