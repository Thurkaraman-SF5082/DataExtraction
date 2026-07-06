using DataExtraction.Interfaces;
using DataExtraction.Services;
using Npgsql;

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
        builder.Services.AddScoped<IDataTableToJsonConvertor, DataTableToJsonConvertor>();
        builder.Services.AddTransient<IDbJSONInsertion, DbJSONInsertion>();
        builder.Services.AddSingleton<QueryBuilder>();
        builder.Services.AddSingleton<JsonQueryBuilder>();
        builder.Services.AddScoped<IAgentTicketInsights, AgentTicketInsights>(cg =>

            new AgentTicketInsights(cg.GetRequiredService<QueryBuilder>()));
        builder.Services.AddScoped<IAgentTicketInsights, AgentJsonTicketInsights>(cg =>

            new AgentJsonTicketInsights(cg.GetRequiredService<JsonQueryBuilder>())
        );

        NpgsqlConnection.GlobalTypeMapper.MapComposite<DateTimeOffset>("timestampz");

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