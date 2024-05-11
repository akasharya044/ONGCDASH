using CIPLV2.Frontdesk.Components;
using CIPLV2.Frontdesk.Components.Dto;
using CIPLV2.Frontdesk.Services;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<ICiplApiService, CiplApiService>(opt => opt.BaseAddress = new Uri(builder.Configuration["ApiUrl"]));
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddSingleton(sp => RabbitConnector.CreateBus(builder.Configuration["RabbitMqHost"]));
builder.Services.AddHostedService<RabbitListner>();
builder.Services.AddSingleton<SessionData>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
