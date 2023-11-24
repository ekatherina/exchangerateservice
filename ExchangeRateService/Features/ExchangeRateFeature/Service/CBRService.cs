using System.Net.Http.Headers;
using System.Xml;
using System.Xml.Serialization;

namespace ExchangeRateService.Features.ExchangeRateFeature.Service
{
    public class CBRService : ICBRService, IDisposable
    {
        private readonly HttpClient _httpClient;

        public CBRService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ValuteCursOnDate> GetCursOnDate(DateTime date, string code)
        {
            var exchangeRates = await GetCursOnDate(date);

            if (exchangeRates.ContainsKey(code))
                return exchangeRates[code];
            else
                throw new CBRException($"Курс валюты {code} на дату {date} не найден");
        }

        //удобно возвращать тут словарь,
        //так в дальнейшем можно поместить этот словарь в кеш и не хожить каждый раз в сервис Центробанка
        public async Task<IDictionary<string, ValuteCursOnDate>> GetCursOnDate(DateTime date)
        {
            var cbrResponse = await SendCrbRequest(date.ToString("yyyy-MM-dd"));

            var result = ConvertCbrResponseToObject(cbrResponse);

            return result;
        }

        private async Task<string> SendCrbRequest(string data)
        {
            try
            {
                var xmlSOAP = $"<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"><soap:Body><GetCursOnDate xmlns=\"http://web.cbr.ru/\"><On_date>{data}</On_date></GetCursOnDate></soap:Body></soap:Envelope>";

                HttpRequestMessage soapMessage = new HttpRequestMessage();
                soapMessage.Method = HttpMethod.Post;
                soapMessage.Content = new StringContent(xmlSOAP);
                soapMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("text/xml; charset=utf-8");

                var response = await _httpClient.SendAsync(soapMessage);
                var responseBodyAsText = await response.Content.ReadAsStringAsync();

                return responseBodyAsText;
            }
            catch (Exception ex)
            {
                throw new CBRException("Запрос к сервису Центробанка завершился с ошибкой", ex);
            }
        }

        private IDictionary<string, ValuteCursOnDate> ConvertCbrResponseToObject(string response)
        {
            IDictionary<string, ValuteCursOnDate> result = new Dictionary<string, ValuteCursOnDate>();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);
                XmlElement root = doc.DocumentElement;
                XmlNodeList nodes = root.SelectNodes("//ValuteCursOnDate");
                foreach (XmlNode node in nodes)
                {
                    var obj = ConvertXmlToObject<ValuteCursOnDate>(node);
                    result.Add(obj.VchCode, obj);
                }
            }
            catch (Exception ex)
            {
                throw new CBRException("Запрос к сервису Центробанка завершился с ошибкой", ex);
            }

            return result;
        }

        private T ConvertXmlToObject<T>(XmlNode xmlNode)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (XmlNodeReader reader = new XmlNodeReader(xmlNode))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        public void Dispose() => _httpClient?.Dispose();
    }
}
