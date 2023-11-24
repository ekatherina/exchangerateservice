using MediatR;

namespace ExchangeRateService.Features.ExchangeRateFeature
{
    public class RateExchangeRequest : IRequest<RateExchangeResponse>
    {
        public DateTime Date { get; set; }
    }
}
