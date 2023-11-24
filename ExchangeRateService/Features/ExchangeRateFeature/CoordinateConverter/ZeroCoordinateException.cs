namespace ExchangeRateService.Features.ExchangeRateFeature.CoordinateConverter
{
    public class ZeroCoordinateException : Exception
    {
        public ZeroCoordinateException() : base("Координата не может быть равна 0") { }
    }
}
