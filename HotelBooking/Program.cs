
using HotelBooking.IRepository;
using HotelBooking.Repository;
using HotelBooking.Configuration;
using HotelBooking.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using HotelBooking.AuthServices;
using Microsoft.Extensions.DependencyInjection;
using HotelBooking.ServiceExtensions;
using HotelBooking.EmailService;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuth, Auth>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddAutoMapper(typeof(MapperInitilizer)); //Setup for DTO's
builder.Services.AddDbContext<DatabaseContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"));
});

var Configuration = builder.Configuration;

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(Configuration);
builder.Services.ConfigSwagger(Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});
var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.Configure<FormOptions>(option => {
    option.ValueLengthLimit = int.MaxValue;
    option.MultipartBodyLengthLimit = int.MaxValue;
    option.MemoryBufferThreshold = int.MaxValue;
});
builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelListings v1"));
}

app.ExceptionHandlerConfiguration();

app.UseSwagger();

app.UseCors("AllowAll");

app.UseRouting();

app.UseHttpsRedirection();

app.MapControllers();

app.UseAuthentication();

app.UseAuthorization();

app.Run();
