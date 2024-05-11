using System.Net;
using System.Text;
using AutoMapper;
using CIPLV2.DAL.Processes;
using CIPLV2.DAL.Unitofworks;
using CIPLV2.Models.Admin;
using CIPLV2.Models.Common;

namespace CIPLV2.DAL.DataService
{
    public class AdminData : IAdminData
    {
        readonly IUnitOfWorks _uow;
        readonly IMapper _mapper;
        public AdminData(IUnitOfWorks uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Response> UpsertTest(TestDTO test)
        {
            Response response = new Response();
            try
            {
                var dbdata = _mapper.Map<Test>(test);
                if (dbdata.Id == 0)
                {
                    await _uow.test.AddAsync(dbdata);
                    await _uow.SaveAsync();
                }
                else
                {
                    _uow.test.Update(dbdata);
                    _uow.Save();
                }

                response.Status = "Success";
                response.Message = "Data Saved";
                response.Data = dbdata;
            }
            catch (Exception ex)
            {
                response.Status = "Failed";
                var errormessage = await _uow.AddException(ex);
                response.Message = errormessage;

            }
            return response;
        }

        public async Task<Response> GetTestList()
        {
            Response response = new Response();
            try
            {
                var dbdata = _uow.test
                    .GetSelectedNoTracking(x => _mapper.Map<TestDTO>(x));
                response.Status = "Success";
                response.Message = "Data Sent";
                response.Data = dbdata;
            }
            catch (Exception ex)
            {
                response.Status = "Failed";
                var errormessage = await _uow.AddException(ex);
                response.Message = errormessage;

            }
            return response;
        }
		public async Task<Response> Login(AdminUsersDto data)
		{

			string Authenticate_URL = "https://ithelpdesknew.ongc.co.in/auth/authentication-endpoint/authenticate/token?TENANTID=748960833";

			Response response = new Response();
			try
			{
				var mappeddata = _mapper.Map<AdminUsers>(data);
				string encryptkey = "CIPL@#ONGC$2023";
				//var dbdata = await _uow.adminUsers.GetFirstOrDefaultAsync(x => x.Name == mappeddata.Name && x.Password == EncryptDecrypt.EncryptData(mappeddata.Password, encryptkey));
				//if (dbdata != null)
				//{
				//LogWriter.LogWrite("Enter into if condition");
				string requestData = "{\"login\":\"" + data.UserName + "\", \"password\":\"" + data.Password + "\"}";
				byte[] requestDataBytes = Encoding.UTF8.GetBytes(requestData);
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Authenticate_URL);
				//LogWriter.LogWrite("after MF api call");
				request.Method = "POST";
				request.ContentType = "application/json";
				request.ContentLength = requestDataBytes.Length;

				// Write the request data to the request stream
				using (Stream requestStream = request.GetRequestStream())
				{
					requestStream.Write(requestDataBytes, 0, requestDataBytes.Length);
				}
				using (HttpWebResponse webresponse = (HttpWebResponse)request.GetResponse())
				{
					if (webresponse.StatusCode == HttpStatusCode.OK)
					{
						StreamReader streamReader = new StreamReader(webresponse.GetResponseStream());
						string responseBody = streamReader.ReadToEnd();
						data.Token = responseBody;
						//LogWriter.LogWrite("print Token");
						//LogWriter.LogWrite(responseBody);

					}
				}

				if (data.Token != null && data.Token != "")
				{
					getToken.LoginToken = data.Token;

					response.Status = "Success";
					//dbdata.Password = EncryptDecrypt.DecryptData(dbdata.Password, encryptkey);
					response.Message = "Login SuccessFully";
					var dtodata = _mapper.Map<AdminUsersDto>(mappeddata);
					var persondata = _uow.personDetails.GetFirstOrDefault(x => x.EmployeeNumber == data.UserName || x.Upn == data.UserName);
					if (persondata != null)
					{
						dtodata.MfuserId = persondata.MfpersonId;
						dtodata.Location = persondata.Location;
						dtodata.FirstName = persondata.FirstName;
					}
					dtodata.Token = data.Token;
					response.Data = dtodata;
				}
				//}
				else
				{
					response.Status = "Failed";
					response.Message = "Login Failed";
					response.Data = null;
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

		public async Task<Response> UpsertAdminUsers(AdminUsersDto data)
        {
            Response response = new Response();
            try
            {
               // LogWriter.LogWrite("Enter in UpsertAdminUsers try block");
                var dbdata = _mapper.Map<AdminUsers>(data);
                string encryptkey = "CIPL@#ONGC$2023";
                dbdata.Password = EncryptDecrypt.EncryptData(data.Password, encryptkey);
                if (dbdata.Id == 0)
                {
                    await _uow.adminUsers.AddAsync(dbdata);
                    await _uow.SaveAsync();
                }
                else
                {
                    _uow.adminUsers.Update(dbdata);
                    _uow.Save();
                }

                response.Status = "Success";
                response.Message = "Data Saved";
                response.Data = dbdata;
            }
            catch (Exception ex)
            {
                //LogWriter.LogWrite("Enter in UpsertAdminUsers catch block");
                //LogWriter.LogWrite(ex.ToString());
                response.Status = "Failed";
                var errormessage = await _uow.AddException(ex);
                response.Message = errormessage;

            }
            return response;
        }
		public async Task<Response> Logindashboard(AdminUsersDto data)
		{

			Response response = new Response();
			try
			{
				var mappeddata = _mapper.Map<AdminUsers>(data);
				string encryptkey = "CIPL@#ONGC$2023";
				var dbdata = await _uow.adminUsers.GetFirstOrDefaultAsync(x => x.Name == mappeddata.Name && x.Password == EncryptDecrypt.EncryptData(mappeddata.Password, encryptkey));
				if (dbdata != null)
				{

					response.Status = "Success";
					dbdata.Password = EncryptDecrypt.DecryptData(dbdata.Password, encryptkey);
					response.Message = "Login SuccessFully";
					var dtodata = _mapper.Map<AdminUsersDto>(dbdata);
					dtodata.Token = data.Token;
					response.Data = dtodata;

				}
				else
				{
					response.Status = "Failed";
					response.Message = "Login Failed";
					response.Data = null;
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
