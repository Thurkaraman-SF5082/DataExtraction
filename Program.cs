using System.Text.Json.Serialization;
using DataExtraction.Interfaces;
using DataExtraction.Models;

namespace DataExtraction;

class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();

        builder.Services.AddScoped<IDBReadService, DBReadService>();
        builder.Services.AddScoped<IDbWriteService, DbWriteService>();
        builder.Services.AddScoped<ICustomField, CustomFieldCode>();
        builder.Services.AddScoped<IPaymentOccurency, PaymentOccurency>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();

    }
}