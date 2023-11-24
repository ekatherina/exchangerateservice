namespace ExchangeRateService.Features.ExchangeRateFeature.Service
{
    public interface ICBRService
    {
        Task<IDictionary<string, ValuteCursOnDate>> GetCursOnDate(DateTime date);

        Task<ValuteCursOnDate> GetCursOnDate(DateTime date, string code);
    }
}
