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
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using System.Net;

namespace swaggerAzFunc1
{
    public class Function1
    {
        private const string Function1Tag = "Function1";
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
        [OpenApiOperation(
            operationId: nameof(GetValues), 
            tags: new [] { Function1Tag },
            Summary = "Gets all available values.",
            Description = @"Returns the set of all available values.
            <pre>GET /api/values</pre>")]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: MediaTypeNames.Application.Json,
            bodyType: typeof(IEnumerable<ValueModel>))]
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

        [FunctionName(nameof(GetValue))]
        [OpenApiOperation(
            operationId: nameof(GetValue), 
            tags: new [] { Function1Tag },
            Summary = "Gets the specified value.",
            Description = @"Returns the value specified by its id.
            <pre>GET /api/values/{valueid:guid}</pre>")]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: MediaTypeNames.Application.Json,
            bodyType: typeof(ValueModel))]
        [OpenApiResponseWithoutBody(
            statusCode: HttpStatusCode.NotFound,
            Description = "Returns 404 NotFound if a value for the specified identifier cannot be found.")]
        public async Task<IActionResult> GetValue(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "values/{valueId:guid}")] HttpRequest request, 
            Guid valueId)
        {
            logger.LogInformation($"{nameof(GetValue)}, {nameof(valueId)}={valueId}");
            Stopwatch stopwatch = Stopwatch.StartNew();

            ValueModel value = await service.Get(valueId);
            if(value == null)
            {
                return new NotFoundResult();
            }

            string json = JsonSerializer.Serialize(value, options);

            stopwatch.Stop();
            logger.LogInformation($"{nameof(GetValues)}, {nameof(valueId)}={valueId}, elapsed={stopwatch.Elapsed}");
            return new ContentResult {
                Content = json,
                ContentType = MediaTypeNames.Application.Json,
                StatusCode = StatusCodes.Status200OK,
            };
        }
    }
}
