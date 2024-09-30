using AMSWebApi.Models;
using AMSWebApi.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AMSWebApi
{
    public class Program
        {
            public static void Main(string[] args)
            {
                var builder = WebApplication.CreateBuilder(args);

                // Add services to the container.

                builder.Services.AddControllers();

                //Register a JWT Authentication schema
                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = builder.Configuration["Jwt:Issuer"],
                            ValidAudience = builder.Configuration["Jwt:Issuer"],
                            IssuerSigningKey = new SymmetricSecurityKey(
                         System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

                        };

                    });


                //0 Json format
                builder.Services.AddControllersWithViews()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.PropertyNamingPolicy = null;
                        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                        options.JsonSerializerOptions.WriteIndented = true;

                    });
                //1- connection string as middleware

                builder.Services.AddDbContext<AssetMsDbContext>(options =>
                 options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));

            //2-  Repository as middleware

               builder.Services.AddScoped<IAssetRepository, AssetRepository>();
               builder.Services.AddScoped<ILoginRepository, LoginRepository>();


                // Learn more about configuring Swagger/OpenAPI at/*
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                // if (app.Environment.IsDevelopment())
                {
                app.UseSwagger();
                app.UseSwaggerUI();
                 }

                app.UseHttpsRedirection();


                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllers();

                app.Run();
            }
        
    }
}
    

