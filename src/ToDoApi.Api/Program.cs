using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Api.Middleware;
using ToDoApi.Application.DTOs;
using ToDoApi.Application.Features.ToDos.Commands;
using ToDoApi.Application.Interfaces.IRepositories;
using ToDoApi.Application.Validators;
using ToDoApi.Infrastructure.Data;
using ToDoApi.Infrastructure.Repositories;
using ToDoApi.Api.Models; 
using Serilog;


namespace ToDoApi.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Serilog Configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration) // Read Serilog config from appsettings.json
                .CreateLogger();

            Log.Information("Starting ToDo API web host.");

            var builder = WebApplication.CreateBuilder(args);

            // --- Serilog Integration with Host Builder
            builder.Host.UseSerilog();

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register the DbContext 
            builder.Services.AddDbContext<AppDbContext>(options =>
             options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register the Repositories
            builder.Services.AddScoped<IToDoRepository, ToDoRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

          
            // Register MediatR
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateToDoCommand).Assembly));

            /*
            // Register the Services 
            builder.Services.AddScoped<IToDoService, ToDoService>();
            builder.Services.AddScoped<IUserService, UserService>();
            */

            // Register FluentValidation ,AutoMapper
            builder.Services.AddFluentValidationAutoValidation();// for automatic model validation
           

            //Register FluentValidation validators - combine way
            //builder.Services.AddValidatorsFromAssemblyContaining<CreateToDoDtoValidator>();

            // Register FluentValidation validators - one by one
            builder.Services.AddScoped<IValidator<CreateToDoDto>, CreateToDoDtoValidator>();
            builder.Services.AddScoped<IValidator<UpdateToDoDto>, UpdateToDoDtoValidator>();
            builder.Services.AddScoped<IValidator<CreateUserDto>, CreateUserDtoValidator>();
            builder.Services.AddScoped<IValidator<UpdateUserDto>, UpdateUserDtoValidator>();

            // IOptions Configuration Registration
            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseSerilogRequestLogging();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //app.UseSerilogRequestLogging();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
