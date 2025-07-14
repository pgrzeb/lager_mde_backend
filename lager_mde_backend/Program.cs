using lager_mde_backend.Data;
using lager_mde_backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSignalR(); //Für ListenerService 
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IArtikelService, ArtikelService>();
builder.Services.AddScoped<IPersonalService, PersonalService>();
builder.Services.AddScoped<IStapaufService, StapaufService>();
builder.Services.AddScoped<ILagstammService, LagstammService>();
builder.Services.AddScoped<ILeermeldService, LeermeldService>();
builder.Services.AddScoped<IStapDisplayService, StapDisplayService>();
builder.Services.AddHostedService<ListenerService>();
builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
    });

builder.Host.UseWindowsService();
builder.WebHost.UseUrls("http://0.0.0.0:5000");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseRouting();
app.UseCors("AllowAll");
//SignalR Middleware 
app.MapHub<DataHub>("/dataHub");
app.UseAuthorization();
app.MapControllers();

app.Run();
