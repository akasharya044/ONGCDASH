using CIPLV2.Models.Admin;
using CIPLV2.Models.Registration;

namespace CIPLV2.DAL.DataService
{
	public interface IMachineRegistrationData
	{
		Task<Response> AddMachineRegistration(MachineRegistrationDto data);
		Task<Response> GetMachinesData();

    }
}
