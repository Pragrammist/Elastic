using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;


namespace Searcher2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
    
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<GrpcSearch.Payment>> Get()
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            using var channel = GrpcChannel.ForAddress("https://localhost:7142");
            var client = new GrpcSearch.SearchService.SearchServiceClient(channel);
            var clResult = await client.SearchAsync(new GrpcSearch.QueryInput {Query = "hello"});
            
            return clResult.Payments;
        }
    }
}