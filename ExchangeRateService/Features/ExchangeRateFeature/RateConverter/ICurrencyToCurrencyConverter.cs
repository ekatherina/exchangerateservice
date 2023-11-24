namespace ExchangeRateService.Features.ExchangeRateFeature.RateConverter
{
    public interface ICurrencyToCurrencyConverter
    {
        double Convert(double rubleRateToForingCurrency);
    }
}
