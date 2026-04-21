using System.Data;
using DataExtraction.Interfaces;
using DataExtraction.Models;
using Npgsql;

namespace DataExtraction;

class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // builder.Services.AddScoped<IDbConnection>(sp =>
        //     new NpgsqlConnection(builder.Configuration.GetConnectionString("PostgresDb")));

        builder.Services.AddScoped<IDBService, DBService>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();

    }
}