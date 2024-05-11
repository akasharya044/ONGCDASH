
using CIPLV2.Models.Admin;
using CIPLV2.Models.Tickets;

namespace CIPLV2.DAL.DataService
{
	public interface ICsatSettingData
	{
		Task<Response> UpsertSetting(CsatSetting data);
		Task<Response> GetCsatSetting();
	}
}
