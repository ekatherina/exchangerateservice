namespace ExchangeRateService.Features.ExchangeRateFeature.Service
{
    public class CBRException : Exception
    {
        public CBRException(string message) : base(message) { }
        public CBRException(string message, Exception inner) : base(message, inner) { }
    }
}
