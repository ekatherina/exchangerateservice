namespace ExchangeRateService.Features.ExchangeRateFeature.CoordinateConverter
{
    public class CoordinateToDateConverter : ICoordinateToDateConverter
    {
        private readonly IConfiguration _config;

        public CoordinateToDateConverter(IConfiguration config)
        {
            _config = config;
        }

        public DateTime Convert(int x, int y)
        {
            if (x == 0 || y ==0)
            {
                throw new ZeroCoordinateException();
            }

            var R = _config.GetValue<int?>("CoordinateConverter:R");
            if (R == null)
            {
                throw new ConfigurationException("Нужно добавить CoordinateConverter:R в файл конфигурации");
            }

            if (x > R || x < -R)
            {
                throw new RLimitException(R.Value);
            }
            if (y > R || y < -R)
            {
                throw new RLimitException(R.Value);
            }

            DateTime today = DateTime.Now;
            if (x < 0 && y < 0) //day before yesterday
            {
                return today.AddDays(-2);
            }
            if (x < 0 && y > 0) //yesterday
            {
                return today.AddDays(-1);
            }
            if (x > 0 && y > 0) //today
            {
                return today;
            }
            if (x > 0 && y < 0) //tomorrow
            {
                return today.AddDays(1);
            }

            throw new Exception("Не получилось определить дату");
        }
    }
}
