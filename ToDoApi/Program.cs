using FluentValidation;
using FluentValidation.AspNetCore;
using ToDoApi.Models.DTOs;
using ToDoApi.Validators;
using ToDoApi.Services;

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
            builder.Services.AddSingleton<IToDoService, ToDoService>();
            builder.Services.AddFluentValidationAutoValidation();// for automatic model validation

            //Register FluentValidation validators
            builder.Services.AddValidatorsFromAssemblyContaining<CreateToDoDtoValidator>();

            // Register FluentValidation validators - one by one
            //builder.Services.AddScoped<IValidator<ToDoRequest>, ToDoRequestValidator>();
            //builder.Services.AddScoped<IValidator<ToDoResponse>, ToDoResponseValidator>();

            var app = builder.Build();

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
