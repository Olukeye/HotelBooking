
using HotelBooking.IRepository;
using HotelBooking.Repository;
using HotelBooking.Utils_OR_ServiceExtensions;
using HotelBooking.Configuration;
using HotelBooking.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using HotelBooking.AuthServices;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;


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
builder.Services.AddScoped<IAuth, Auth>();
builder.Services.AddAutoMapper(typeof(MapperInitilizer)); //Setup for DTO's
builder.Services.AddDbContext<DatabaseContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"));
});

builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(config);

builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Description =  @"JWT Authorization header using Bearer scheme. 
          Enter 'Bearer' [space] and then your token in the input below.
          Example: 'Bearer 0bchyie8gmvy9876534567igx6f6s56qr81hvvbllsxz'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            },
            Scheme = "0auth2",
            Name = "Bearer",
            In = ParameterLocation.Header
        },
            new List<string>()
        }
    });
    
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelListings", Version = "v1" });
});

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

app.ExceptionHandlerConfiguration();


app.UseSwagger();

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();
