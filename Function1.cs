using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Net.Http;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;

namespace swaggerAzFunc1
{
    public class Function1
    {
        private readonly ILogger<Function1> logger;
        private readonly JsonSerializerOptions options;
        private readonly HttpClient http;
        private readonly IValuesService service;
        
        public Function1(ILogger<Function1> logger, JsonSerializerOptions options, HttpClient http, IValuesService service)
        {
            this.logger = logger;
            this.options = options;
            this.http = http;
            this.service = service;
        }

        [FunctionName(nameof(GetValues))]
        public async Task<IActionResult> GetValues(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "values")] HttpRequest request)
        {
            logger.LogInformation($"{nameof(GetValues)}");
            Stopwatch stopwatch = Stopwatch.StartNew();

            // string name = request.Query["name"];

            // string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            // dynamic data = JsonSerializer.Deserialize(requestBody);
            // name = name ?? data?.name;

            // string responseMessage = string.IsNullOrEmpty(name)
            //     ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            //     : $"Hello, {name}. This HTTP triggered function executed successfully.";

            IEnumerable<ValueModel> values = await service.Get();
            string json = JsonSerializer.Serialize(values, options);

            stopwatch.Stop();
            logger.LogInformation($"{nameof(GetValues)}, count={values?.Count()}, elapsed={stopwatch.Elapsed}");
            return new ContentResult {
                Content = json,
                ContentType = MediaTypeNames.Application.Json,
                StatusCode = StatusCodes.Status200OK,
            };
        }
    }
}
