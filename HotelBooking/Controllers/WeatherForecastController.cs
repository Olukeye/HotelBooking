using HotelBooking.EmailService;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
        private readonly IEmailSender _emailSender;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IEmailSender emailSender)
        {
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpPost(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Post()
        {
            var files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();
            var message = new Message(new string[] { "seunolukeye2000@gmail.com" }, "Test email", "This is the content from our email.", null);
            await _emailSender.SendEmailAsync(message);

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}