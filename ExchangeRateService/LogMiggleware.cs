using Serilog;

namespace ExchangeRateService
{
    public class LogMiggleware
    {
        private readonly RequestDelegate _next;

        public LogMiggleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            finally
            {
                Log.Information($"Request {context.Request?.Method} {context.Request?.Path.Value} {context.Request?.QueryString.ToString()} => {context.Response?.StatusCode}");
            }
        }
    }
}
