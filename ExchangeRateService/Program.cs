using ExchangeRateService.Features.ExchangeRateFeature.CoordinateConverter;
using ExchangeRateService.Features.ExchangeRateFeature.RateConverter;
using ExchangeRateService.Features.ExchangeRateFeature.Service;
using Serilog;

namespace ExchangeRateService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                        .WriteTo.File(builder.Configuration["Serilog:FileName"])
                        .CreateLogger();

            builder.Services.AddTransient<ICoordinateToDateConverter, CoordinateToDateConverter>();
            builder.Services.AddTransient<ICurrencyToCurrencyConverter, CurrencyToCurrencyConverter>();

            builder.Services.AddSingleton<ICBRService, CBRService>();
            builder.Services.AddHttpClient<ICBRService, CBRService>(httpClient =>
            {
                httpClient.BaseAddress = new Uri(builder.Configuration["CBRService:Url"]);
            });

            builder.Services.AddControllers();

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

            var app = builder.Build();

            app.UseMiddleware<LogMiggleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}