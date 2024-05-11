
namespace CIPLV2.DAL.DataService
{
	public interface IDataService
	{
		IAdminData adminData { get; }
		ITicketData ticketData { get; }
		IMachineRegistrationData machineRegistrationData { get; }
		ICategoriesData categoriesData { get; }
		ISubCategoriesData subCategoriesData { get; }
		IPersonDetailsData personDetailsData { get; }
		IDeviceDetailsData deviceDetailsData { get; }
		IAreaData areadata { get; }
		IEventHistories eventHistory { get; }
		IUserSystemSoftwareData userSystemSoftwareData { get; }
		ICsatSettingData csatsetting { get; }
		IIncidentData incident { get; set; }
		ISearchQuestionData searchQuestion { get; set; }
		IAdditionalInfo additionalInfo { get; set; }
	}
}
