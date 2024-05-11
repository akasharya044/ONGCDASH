using CIPLV2.DAL.DatabaseContexts;
using CIPLV2.DAL.Processes;
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
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Hosting;

namespace CIPLV2.DAL.Unitofworks
{
    public class UnitOfWorks : IDisposable, IUnitOfWorks
    {
        readonly Ciplv2DbContext _context;
        private IDbContextTransaction transaction = null;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private bool disposed = false;
 
        public UnitOfWorks(Ciplv2DbContext dbcontext, IHostEnvironment hostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _context = dbcontext;
            _hostEnvironment = hostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            test = new Repository<Test>(_context);
            ticketrecord = new Repository<TicketRecord>(_context);
			machineRegistration = new Repository<MachineRegistration>(_context);
			categories = new Repository<Categories>(_context);
			subCategories = new Repository<SubCategories>(_context);
			questions = new Repository<Questions>(_context);
			answers = new Repository<Answers>(_context);
			personDetails = new Repository<PersonDetails>(_context);
			deviceDetails = new Repository<DeviceDetails>(_context);
			area = new Repository<Areas>(_context);
            deviceRunningLog = new Repository<DeviceRunningLog>(_context);
			adminUsers = new Repository<AdminUsers>(_context);
			eventHistory = new Repository<EventHistory>(_context);
			starRating = new Repository<StarRating>(_context);
			events = new Repository<Events>(_context);
			UserSystemSoftware = new Repository<UserSystemSoftware>(_context);
			UserSystemHardware = new Repository<UserSystemHardware>(_context);
            incident = new Repository<Incident>(_context);
			csatsetting = new Repository<CsatSetting>(_context);
			searchQuestion = new Repository<SearchQuestion>(_context);
			deviceDailyStatus = new Repository<DeviceDailyStatus>(_context);
			additionalInformation = new Repository<AdditionalInformation>(_context);
			harddiskinfo = new Repository<AdditionalInformationHardDisk>(_context);
			noservice = new Repository<NoServices>(_context);
			deviceData = new Repository<DeviceData>(_context);
		}
        public IRepository<Test> test { get; private set; }
        public IRepository<AdminUsers> adminUsers { get; private set; }
        public IRepository<TicketRecord> ticketrecord { get; private set; }
        public IRepository<MachineRegistration> machineRegistration { get; private set; }
		public IRepository<Categories> categories { get; private set; }
		public IRepository<SubCategories> subCategories { get; private set; }
		public IRepository<Questions> questions { get; private set; }
		public IRepository<Answers> answers { get; private set; }
		public IRepository<PersonDetails> personDetails { get; private set; }
		public IRepository<DeviceDetails> deviceDetails { get; private set; }
		public IRepository<Areas> area { get; private set; }
        public IRepository<DeviceRunningLog> deviceRunningLog { get; private set; }
		public IRepository<EventHistory> eventHistory { get; private set; }
		public IRepository<StarRating> starRating { get; private set; }
		public IRepository<Events> events { get; private set; }
		public IRepository<UserSystemSoftware> UserSystemSoftware { get; set; }
		public IRepository<UserSystemHardware> UserSystemHardware { get; set; }
        public IRepository<Incident> incident { get; set; }
        public IRepository<CsatSetting> csatsetting { get; set; }
		public IRepository<SearchQuestion> searchQuestion { get; set; }
		public IRepository<DeviceDailyStatus> deviceDailyStatus { get; set; }
		public IRepository<AdditionalInformation> additionalInformation { get; set; }

		public IRepository<AdditionalInformationHardDisk> harddiskinfo { get; set; }

		public IRepository<NoServices> noservice { get; set; }

		public IRepository<DeviceData> deviceData { get; set; }
		public async Task<string> AddException(Exception exception)
        {
            string output = _hostEnvironment.EnvironmentName == "Production" ? "Technical Error - Contact Admin" : exception.Message.ToString();
            try
            {
                var endpoint = _httpContextAccessor.HttpContext.Request.Path;
                var method = _httpContextAccessor.HttpContext.Request.Method;
                string body = "";
                body += GetBodyString();

                if (_httpContextAccessor.HttpContext.Request.QueryString != null)
                {
                    body += _httpContextAccessor.HttpContext.Request.QueryString.ToString();
                }

                //Add table to add exception
                ExceptionLog exceptionLog = new ExceptionLog();
                exceptionLog.Logdate = DateTime.Now;
                exceptionLog.ExceptionMessage = exception.Message;
                exceptionLog.ExceptionSource = exception.Source;
                exceptionLog.ExceptionURL = endpoint;
                exceptionLog.RequestData = body;
                exceptionLog.ActionMethod = method;

                //Create exception object and save it to DB
                await _context.ExceptionLogs.AddAsync(exceptionLog);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                LogWriter.LogWrite(exception.Message);

            }
            return output;
        }
        private string GetBodyString()
        {
            try
            {
                string body = "";


                _httpContextAccessor.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);
                using (StreamReader stream = new StreamReader(_httpContextAccessor.HttpContext.Request.Body))
                {

                    body = stream.ReadToEnd();

                    // body = "param=somevalue&param2=someothervalue"
                }
                return body;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        public async Task BeginTrans()
        {
            transaction = _context.Database.BeginTransaction();
        }
        public async Task Commit()
        {
            transaction.Commit();
        }
        public async Task RollBackTrans()
        {
            transaction.Rollback();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void ClearTracker()
        {
            _context.ChangeTracker.Clear();
        }
    }
}
