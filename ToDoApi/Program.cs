using FluentValidation;
using FluentValidation.AspNetCore;
using ToDoApi.Models.DTOs;
using ToDoApi.Validators;
using ToDoApi.Services;
using ToDoApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;

namespace ToDoApi
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
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IToDoService, ToDoService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddFluentValidationAutoValidation();// for automatic model validation
            builder.Services.AddDbContext<AppDbContext>(options =>
              options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            //Register FluentValidation validators - combine way
        //    builder.Services.AddValidatorsFromAssemblyContaining<CreateToDoDtoValidator>();

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
