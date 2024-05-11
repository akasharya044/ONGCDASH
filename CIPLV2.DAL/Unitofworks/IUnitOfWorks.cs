using CIPLV2.Models.AdditionalInfo;
using CIPLV2.Models.Admin;
using CIPLV2.Models.Area;
using CIPLV2.Models.Category;
using CIPLV2.Models.DeviceDetail;
using CIPLV2.Models.EventHistories;
using CIPLV2.Models.Incidents;
using CIPLV2.Models.PersonDetail;
using CIPLV2.Models.Registration;
using CIPLV2.Models.SearchQuestion;
using CIPLV2.Models.SubCategory;
using CIPLV2.Models.Tickets;
using CIPLV2.Models.UserSystemHardware;
using CIPLV2.Models.UserSystemSoftware;

namespace CIPLV2.DAL.Unitofworks
{
    public interface IUnitOfWorks 
    {
        IRepository<Test> test {  get; } 
        IRepository<TicketRecord> ticketrecord { get; }
        IRepository<MachineRegistration> machineRegistration { get; }
		IRepository<Categories> categories { get; }
		IRepository<SubCategories> subCategories { get; }
		IRepository<Questions> questions { get; }
		IRepository<Answers> answers { get; }
		IRepository<PersonDetails> personDetails { get; }
		IRepository<DeviceDetails> deviceDetails { get; }
		IRepository<Areas> area { get; }
        IRepository<DeviceRunningLog> deviceRunningLog { get; }
		IRepository<AdminUsers> adminUsers { get; }
		IRepository<EventHistory> eventHistory { get; }
		IRepository<StarRating> starRating { get; }
		IRepository<Events> events { get; }
		IRepository<UserSystemSoftware> UserSystemSoftware { get; set; }
		IRepository<UserSystemHardware> UserSystemHardware { get; set; }
        IRepository<Incident> incident { get; set; }
        IRepository<CsatSetting> csatsetting { get; set; }
		IRepository<SearchQuestion> searchQuestion { get; set; }
		IRepository<DeviceDailyStatus> deviceDailyStatus { get; set; }
		IRepository<AdditionalInformation> additionalInformation { get; set; }

		IRepository<AdditionalInformationHardDisk> harddiskinfo { get; set; }

		IRepository<NoServices> noservice { get; set; }

		IRepository<DeviceData> deviceData { get; set; }
		Task SaveAsync();
        void Save();
        void ClearTracker();
        Task BeginTrans();
        Task Commit();
        Task RollBackTrans();
        Task<string> AddException(Exception exception);
    }
}
