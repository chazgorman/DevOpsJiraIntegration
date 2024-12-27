using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace lms
{
    public class DevOpsItemCreated
    {
        private readonly ILogger<DevOpsItemCreated> _logger;

        public DevOpsItemCreated(ILogger<DevOpsItemCreated> logger)
        {
            _logger = logger;
        }

        //[Function("DevOpsItemCreated")]
        //public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        //{
        //    _logger.LogInformation("C# HTTP trigger function processed a request.");
        //    return new OkObjectResult("Welcome to LMS Azure Functions!");
        //}

        [Function("DevOpsItemCreated")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            _logger.LogInformation("ThirdParty Payload has been captured...");

            string requestBody = new StreamReader(req.Body).ReadToEndAsync().Result;
            dynamic data = JsonConvert.ToString(requestBody);

            string messageContent = $"{data}";

            HttpClient client = new HttpClient();

            client.PostAsync("https://webhook-test.com/644eadb4db900ca09c8ec44ad50fa865", data);

            //var responseString = await response.Content.ReadAsStringAsync();
            return new OkObjectResult(messageContent);
        }
    }
}
