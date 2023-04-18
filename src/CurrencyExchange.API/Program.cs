using CurrencyExchange.API.Validators;
using CurrencyExchange.Application.Repositories;
using CurrencyExchange.Core.Interfaces;
using CurrencyExchange.Core.Services;
using CurrencyExchange.Core.Settings;
using CurrencyExchange.Infrastructure.Caching;
using CurrencyExchange.Infrastructure.DataAccess;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CurrencyExchangeDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

// Fluent validation setup 
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CurrencyExchangeRequestValidator>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<CurrencyExchangeDbContextInitialiser>();

builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();
builder.Services.AddScoped<IExchangeRateCache, ExchangeRateCache>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IExchangeService, ExchangeService>();


// Register ExchangeRateProviderSettings
builder.Services.Configure<ExchangeRateProviderSettings>(builder.Configuration.GetSection("ExchangeRateProvider"));


builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Initialise and seed database
    using (var scope = app.Services.CreateScope())
    {
        var initialiser = scope.ServiceProvider.GetRequiredService<CurrencyExchangeDbContextInitialiser>();
        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
