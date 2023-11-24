namespace ExchangeRateService.Features.ExchangeRateFeature.RateConverter
{
    //Так как в задании написано: "значение иностранной валюты выводить в значении к одному рублю РФ"
    //Нужно выводить 1р - 0.01USD, а получаем мы курс наоборот 1USD - 100р

    public class CurrencyToCurrencyConverter: ICurrencyToCurrencyConverter
    {
        public double Convert(double rubleRateToForingCurrency)
        {
            if (rubleRateToForingCurrency == 0)
                return 0;
            return Math.Round(1 * 1 / rubleRateToForingCurrency, 2); //решение с помощью пропоции
        }
    }
}
