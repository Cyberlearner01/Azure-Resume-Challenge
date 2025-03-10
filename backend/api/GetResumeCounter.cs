using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Extensions.Http;
using Microsoft.Azure.Functions.Worker.Extensions.CosmosDB;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.CosmosDB;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Company.Function
{
    public class GetResumeCounter
    {
        private readonly ILogger<GetResumeCounter> _logger;

        public GetResumeCounter(ILogger<GetResumeCounter> logger)
        {
            _logger = logger;
        }

        [FunctionName("GetResumeCounter")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req, // Use HttpRequestData
            [CosmosDB(
                databaseName: "AzureResume",
                collectionName: "Counter",
                ConnectionStringSetting = "AzureResumeConnectionString",
                Id = "1",
                PartitionKey = "1")] Counter counter,   // Input binding to get current counter

            [CosmosDB(
                databaseName: "AzureResume",
                collectionName: "Counter",
                ConnectionStringSetting = "AzureResumeConnectionString")] IAsyncCollector<Counter> updatedCounter)  // Output binding to save updated counter

        {
            // Log the request
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            // If no counter document found, return an error response
            if (counter == null)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Counter document not found.")
                };
            }

            // Update the counter
            counter.Count += 1; // Increment counter value

            // Output binding to save the updated counter
            await updatedCounter.AddAsync(counter);

            // Serialize the updated counter to JSON
            var jsonToReturn = JsonConvert.SerializeObject(counter);

            // Return HTTP response with the updated counter
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(jsonToReturn, Encoding.UTF8, "application/json")
            };
        }
    }
}
