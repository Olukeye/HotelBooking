﻿using HotelBooking.Model;
using HotelBooking.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Connections.Features;
using HotelBooking.ResponsHanlder;
using Serilog;
using Microsoft.OpenApi.Models;

namespace HotelBooking.ServiceExtensions
{
    public static class ServiceExtensions
    {
        //Configuration for user Identity
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<ApiUser>(q => 
            { 
                q.User.RequireUniqueEmail = true;
                q.SignIn.RequireConfirmedEmail = true;
                //q.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";
            });

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            builder.AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders()
                .AddDefaultTokenProviders();
                //.AddTokenProvider<EmailConfirmationTokenProvider<ApiUser>>("emailconfirmation");
        }

        //Configuration method for jwtSecret
        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt");
            var key = Environment.GetEnvironmentVariable("KEY");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidIssuer = jwtSettings.GetSection("Issuer").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                };
            });
        }

        //Configuration method for Handling global Error
        public static void ExceptionHandlerConfiguration(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/Json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        Log.Error($"Something Went Wrong In {contextFeature.Error}");
                        await context.Response.WriteAsync(new ResponseStatus()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error..."
                        }.ToString());
                    }
                });
            });
        }

        //Configuration method for SwaggerGen
        public static void ConfigSwagger(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using Bearer scheme. 
                    Enter 'Bearer' [space] and then your token in the input below.
                    Example: 'Bearer Oboy it worked o'",
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
                    }
                );
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelListings", Version = "v1" });
            });
        }
    }
}
