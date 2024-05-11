
using CIPLV2.Models.Admin;
using CIPLV2.Models.PersonDetail;

namespace CIPLV2.DAL.DataService
{
	public interface IPersonDetailsData
	{
		Task<Response> AddPersonDetails(List<PersonDetailsDto> data);
		Task<Response> GetPersonDetails();
		Task<Response> SearchPersonDetails(string value);

    }
}
