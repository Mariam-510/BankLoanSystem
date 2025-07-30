
using Microsoft.OpenApi.Models;
using BankLoanSystem.Application;
using BankLoanSystem.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using BankLoanSystem.API.Middlewares;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using BankLoanSystem.Core.Models.ResponseModels;
using BankLoanSystem.Application.CQRS.Handlers.LoanType;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace BankLoanSystem.API
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            //Swagger
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Loan System API", Version = "v1" });
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme

                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference=new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id=JwtBearerDefaults.AuthenticationScheme
                            },
                            Scheme="Oauth2",
                            Name=JwtBearerDefaults.AuthenticationScheme,
                            In=ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            //Cors
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                    policy
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                );
            });

            //Service Collection
            builder.Services.AddApplication();

            builder.Services.AddInfrastructure(builder.Configuration);


            //Serilog
             Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
           

            builder.Host.UseSerilog();

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllLoanTypesHandler).Assembly));


            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    return new BadRequestObjectResult(
                        ApiResponse<object>.ErrorResponse(errors: actionContext.ModelState.Values.SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage).ToList()));
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        var response = ApiResponse<object>.ErrorResponse(
                            "An unexpected error occurred",
                            500,
                            new List<string> { contextFeature.Error.Message });

                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    }
                });
            });


            app.UseStaticFiles();

            app.UseMiddleware<LoggingMiddleware>();

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }   
    }
}
