namespace ExchangeRateService.Features.ExchangeRateFeature.CoordinateConverter
{
    public class RLimitException : Exception
    {
        public RLimitException(int R) : base($"Координата не попала в окружность радиуса {R}") { }
    }
}
