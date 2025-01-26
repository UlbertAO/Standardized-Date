using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StandardizedDate.Properties.Model.Request;
using StandardizedDate.Properties.Model.Response;

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
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                StandardizedDateInput inputData = JsonConvert.DeserializeObject<StandardizedDateInput>(requestBody);

                _logger.LogInformation($"Standardizing \"{inputData.inputDate}\"");

                var result = new StandardizedDateHelper().ConvertToStandardFormat(inputData.inputDate);

                return new OkObjectResult(JsonConvert.SerializeObject(new StandardizedDateOutput() { date = result }));
            }
            catch (FormatException ex)
            {
                return new BadRequestObjectResult(JsonConvert.SerializeObject(new StandardizedDateOutput() { msg = ex.Message }));
            }
            catch (ArgumentException ex)
            {
                return new BadRequestObjectResult(JsonConvert.SerializeObject(new StandardizedDateOutput() { msg = ex.Message }));

            }
            catch (Exception ex)
            {
                var result = new ObjectResult(ex.Message)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
                return result;
            }


        }
    }
}
