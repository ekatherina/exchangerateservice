using ExchangeRateService.Features.ExchangeRateFeature.RateConverter;
using ExchangeRateService.Features.ExchangeRateFeature.Service;
using MediatR;

namespace ExchangeRateService.Features.ExchangeRateFeature
{
    public class RateExchangeHandler : IRequestHandler<RateExchangeRequest, RateExchangeResponse>
    {
        private readonly IConfiguration _config;
        private readonly ICBRService _cbrService;
        private readonly ICurrencyToCurrencyConverter _ccConverter;

        public RateExchangeHandler(IConfiguration config, ICBRService cbrService, ICurrencyToCurrencyConverter ccConverter)
        {
            _cbrService = cbrService;
            _config = config;
            _ccConverter = ccConverter;
        }

        async Task<RateExchangeResponse> IRequestHandler<RateExchangeRequest, RateExchangeResponse>.Handle(RateExchangeRequest request, CancellationToken cancellationToken)
        {
            var code = _config.GetValue<string>("CBRService:CurrencyCode");
            var rate = await _cbrService.GetCursOnDate(request.Date, code);
            var rateTo1Rubl = _ccConverter.Convert(rate.VunitRate);
            return new RateExchangeResponse()
            {
                Description = $"Значение иностранной валюты в значении к одному рублю РФ, т.е 1Р = {rateTo1Rubl} {code}",
                Date = request.Date,
                Code = code,
                Rate = rateTo1Rubl
            };
        }
    }
}
