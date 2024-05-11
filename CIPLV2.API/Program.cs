using AutoMapper;
using CIPLV2.API.Endpoints;
using CIPLV2.DAL.AutoMappers;
using CIPLV2.DAL.DatabaseContexts;
using CIPLV2.DAL.DataService;
using CIPLV2.DAL.Processes;
using CIPLV2.DAL.Unitofworks;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
 
    builder.Services.AddDbContextPool<Ciplv2DbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Ciplv2DbConnection"), options => options.EnableRetryOnFailure())
                                                               .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).EnableSensitiveDataLogging());
   
 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
var mapperconfig = new MapperConfiguration(x =>
{
    x.AddProfile(new AdminMapper());
    x.AddProfile(new TicketMappers());
    x.AddProfile(new MachineRegistrationMapper());
    x.AddProfile(new CategoriesMapper());
    x.AddProfile(new SubCategoriesMapper());
    x.AddProfile(new PersonDetailsMapper());
    x.AddProfile(new DeviceDetailsMapper());
	x.AddProfile(new AreaMappers());
	x.AddProfile(new EventHistoriesMapper());
	x.AddProfile(new UserSystemSoftwareMapper());
    x.AddProfile(new IncidentMapper());

});
IMapper mapper = mapperconfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddSingleton(sp => RabbitConnector.CreateBus(builder.Configuration["RabbitMqHost"]));
builder.Services.AddSingleton<DeviceLogs>();
//builder.Services.AddHostedService<BackgroundProcesses>();
builder.Services.AddHostedService<RabbitListner>();
builder.Services.AddScoped<IUnitOfWorks, UnitOfWorks>();
builder.Services.AddScoped<IDataService,DataService>();
builder.Services.AddScoped<ISieveProcessor, SieveProcessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//To get request data in exception handling
app.Use((context, next) =>
{
    context.Request.EnableBuffering();
    return next();
});
app.UseHttpsRedirection();

app.MapGroup("api/tickets").MapTicketEndpoint().WithTags("Tickets API");
app.MapGroup("api/machine").MapMachineRegistrationEndpoint().WithTags("Machine Registration API");
app.MapGroup("api/categories").MapCategoryEndpoint().WithTags("Categories API");
app.MapGroup("api/subcategories").MapSubCategoryEndpoint().WithTags("SubCategories API");
app.MapGroup("api/persondetails").MapPersonDetailsEndpoint().WithTags("PersonDetails API");
app.MapGroup("api/devicedetails").MapDeviceDetailsEndpoint().WithTags("DeviceDetails API");
app.MapGroup("api/area").MapAreaEndpoint().WithTags("Area API");
app.MapGroup("api/admin").MapAdminEndpoint().WithTags("Admin API");
app.MapGroup("api/eventHistory").MapEventHistoryMapEndpoint().WithTags("EventHistory API");
app.MapGroup("api/system").MapSystemInfoEndpoint().WithTags("System Info API");
app.MapGroup("api/incident").MapIncidentEndpoint().WithTags("Incident API");
app.MapGroup("api/csatsetting").MapCsatEndpoint().WithTags("CsatSetting API");
app.MapGroup("api/question").MapSearchQuestionEndpoint().WithTags("Question API");
app.MapGroup("api/additional").MapAdditionalEndpoint().WithTags("Additional API");




app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
