namespace ExchangeRateService.Features.ExchangeRateFeature
{
    public class RateExchangeResponse
    {
        public string Description { get; set; }

        public DateTime Date { get; set; }

        public string Code { get; set; }

        public double Rate { get; set; }
    }
}
