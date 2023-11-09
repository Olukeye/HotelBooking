
using HotelBooking.IRepository;
using HotelBooking.Repository;
using HotelBookings.Configuration;
using HotelBookings.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;



var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    // Logs are written in JSON
    .WriteTo.File(
        path: "c:\\HotelBookings\\logs\\log-.txt",
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:1j}{NewLine} {Exception}",
        restrictedToMinimumLevel: LogEventLevel.Information
      )

    // Add a log file that will be replaced by a new log file each day
    .WriteTo.File("all-daily-.logs",
        rollingInterval: RollingInterval.Day)

    // Set default minimum log level
    .MinimumLevel.Debug()

    // Create the actual logger
    .CreateLogger();
try
{
    Log.Information("Hello, World!");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application Failed To Start");
}
finally
{
    Log.CloseAndFlush();
}


// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddDbContext<DatabaseContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"));
});
builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelListings", Version = "v1" });
});


//var connectionString = builder.Configuration;
builder.Services.AddAutoMapper(typeof(MapperInitilizer)); //Setup for DTO's



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseSwagger();

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();
