using AutoMapper;
using CIPLV2.DAL.Processes;
using CIPLV2.DAL.Unitofworks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sieve.Services;

namespace CIPLV2.DAL.DataService
{
    public class DataService : IDataService
    { 
        public DataService(IUnitOfWorks UOW, IMapper mapper, IHostEnvironment hostEnvironment, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory,DeviceLogs deviceLogs, ISieveProcessor sieveProcessor)
        {
            adminData = new AdminData(UOW, mapper);
            ticketData=new TicketData(UOW,mapper,configuration, serviceScopeFactory);
            machineRegistrationData=new MachineRegistrationData(UOW,mapper);
            categoriesData = new CategoriesData(UOW,mapper);
            subCategoriesData = new SubCategoriesData(UOW,mapper);
            personDetailsData = new PersonDetailsData(UOW,mapper);
            deviceDetailsData = new DeviceDetailsData(UOW,mapper,deviceLogs);
			areadata = new AreaData(UOW, mapper);
			eventHistory = new EventHistories(UOW, mapper);
			userSystemSoftwareData = new UserSystemSoftwareData(mapper, UOW);
            incident = new IncidentData(UOW, sieveProcessor, mapper);
			csatsetting = new CsatSettingData(UOW, mapper);
			searchQuestion = new SearchQuestionData(UOW, mapper);
			additionalInfo = new AdditionalInfo(UOW, mapper);
		}

        public IAdminData adminData {get; private set;}
        public ITicketData ticketData { get; private set;}
        public IMachineRegistrationData machineRegistrationData { get; private set;}
		public ICategoriesData categoriesData { get; private set; }
		public ISubCategoriesData subCategoriesData { get; private set; }
		public IPersonDetailsData personDetailsData { get; private set; }
		public IDeviceDetailsData deviceDetailsData { get; private set; }
		public IAreaData areadata { get; private set; }
		public IEventHistories eventHistory { get; set; }
		public IUserSystemSoftwareData userSystemSoftwareData { get; private set; }
        public IIncidentData incident { get; set; }
        public ICsatSettingData csatsetting { get; set; }
		public ISearchQuestionData searchQuestion { get; set; }

		public IAdditionalInfo additionalInfo { get; set; }

	}
}
