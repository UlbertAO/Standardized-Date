using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace StandardizedDate
{
    public class StandardizedDate
    {
        private readonly ILogger<StandardizedDate> _logger;

        public StandardizedDate(ILogger<StandardizedDate> logger)
        {
            _logger = logger;
        }

        [Function("standardizedDate")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
