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
using Microsoft.EntityFrameworkCore;

namespace CIPLV2.DAL.DatabaseContexts
{
	public class Ciplv2DbContext : DbContext
	{
		public Ciplv2DbContext(DbContextOptions<Ciplv2DbContext> options) : base(options)
		{

		}
		public DbSet<Test> Tests { get; set; }
		public DbSet<TicketRecord> TicketRecords { get; set; }
		public DbSet<ExceptionLog> ExceptionLogs { get; set; }
		public DbSet<MachineRegistration> MachineRegistration { get; set; }
		public DbSet<Categories> Categories { get; set; }
		public DbSet<SubCategories> SubCategories { get; set; }
		public DbSet<Questions> Questions { get; set; }
		public DbSet<Answers> Answers { get; set; }
		public DbSet<PersonDetails> PersonDetails { get; set; }
		public DbSet<DeviceDetails> DeviceDetails { get; set; }
		public DbSet<Areas> Areas { get; set; }
		public DbSet<DeviceRunningLog> DeviceRunningLog { get; set; }
		public DbSet<AdminUsers> AdminUsers { get; set; }
		public DbSet<StarRating> StarRating { get; set; }
		public DbSet<EventHistory> EventHistory { get; set; }
		public DbSet<Events> Events { get; set; }
		public DbSet<UserSystemSoftware> UserSystemSoftware { get; set; }
		public DbSet<UserSystemHardware> userSystemHardwares { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<CsatSetting> CsatSetting { get; set; }
		public DbSet<SearchQuestion> SearchQuestions { get; set; }
		public DbSet<DeviceDailyStatus> DeviceDailyStatus { get; set; }
		public DbSet<AdditionalInformation> AdditionalInformation { get; set; }

		public DbSet<AdditionalInformationHardDisk> HardDisks { get; set; }

		public DbSet<NoServices> NoServices { get; set; }

		public DbSet<DeviceData> DeviceData { get; set; }
	}
}
