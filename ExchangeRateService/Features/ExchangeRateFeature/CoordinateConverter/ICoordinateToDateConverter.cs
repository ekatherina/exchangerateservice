namespace ExchangeRateService.Features.ExchangeRateFeature.CoordinateConverter
{
    public interface ICoordinateToDateConverter
    {
        DateTime Convert(int x, int y);
    }
}
