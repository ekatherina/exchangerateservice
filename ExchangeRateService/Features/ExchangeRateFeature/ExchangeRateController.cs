using ExchangeRateService.Features.ExchangeRateFeature.CoordinateConverter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ExchangeRateService.Features.ExchangeRateFeature
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly ICoordinateToDateConverter _cdtConverter;

        public ExchangeRateController(
                                        IMediator mediator,
                                        ICoordinateToDateConverter cdtConverter)
        {
            _mediator = mediator;
            _cdtConverter = cdtConverter;
        }

        [HttpGet]
        public async Task<ActionResult<RateExchangeResponse>> Get(int x, int y)
        {
            try
            {
                //Идея: Подозреваю, что преобразование из координат в дату связано с UI компонентом в приложении.
                //Тогда нужно возложить ответственность за преобразование координаты на экране в дату на фронтенд.
                //Наше апи должно принимать вместо x,y дату.
                //Ну а пока, вот так:
                var date = _cdtConverter.Convert(x, y);

                var response = await _mediator.Send(new RateExchangeRequest() { Date = date });

                return Ok(response);
            }
            catch (ZeroCoordinateException zEx)
            {
                Log.Warning(zEx.ToString());

                return BadRequest(zEx.Message); //400
            }
            catch (RLimitException rEx)
            {
                Log.Warning(rEx.ToString());

                return BadRequest(rEx.Message); //400
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());

                return Problem(ex.Message); //500
            }
        }
    }
}