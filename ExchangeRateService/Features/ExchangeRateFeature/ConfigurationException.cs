namespace ExchangeRateService.Features.ExchangeRateFeature
{
    public class ConfigurationException : Exception
    {
        public ConfigurationException() : base() { }
        public ConfigurationException(string message) : base(message) { }
        public ConfigurationException(string message, Exception inner) : base(message, inner) { }
    }
}
