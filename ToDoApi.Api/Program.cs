using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Application.DTOs;
using ToDoApi.Application.Interfaces.IRepositories;
//using ToDoApi.Application.Interfaces.Services;
//using ToDoApi.Application.Services;
using ToDoApi.Application.Validators;
using ToDoApi.Infrastructure.Data;
using ToDoApi.Infrastructure.Repositories;
using System.Reflection;
using MediatR;
using ToDoApi.Application.Features.ToDos.Commands;

namespace ToDoApi.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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


            var app = builder.Build();

            app.UseExceptionHandler("/error");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
