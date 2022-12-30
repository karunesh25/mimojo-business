using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;

namespace Mimojo.Business
{
    public static class GetWeatherReport
    {
        [FunctionName("GetWeatherReport")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string finalResponse = string.Empty;
            try
            {
                log.LogInformation("GetWeatherReport HTTP trigger function processed a request.");

                string name = req.Query["name"];
                name = name ?? "dubai";
                log.LogInformation("GetWeatherReport request params : " + name);
                // Call weather  API
                HttpClient newClient = new HttpClient();
                HttpRequestMessage newRequest = new HttpRequestMessage(HttpMethod.Get, string.Format(System.Environment.GetEnvironmentVariable("WeatherApi"), name));

                //Read Server Response
                HttpResponseMessage response = await newClient.SendAsync(newRequest);
                finalResponse = response.Content.ReadAsStringAsync().Result;
                log.LogInformation("Response from api : " + finalResponse);
                finalResponse = System.Text.RegularExpressions.Regex.Unescape(finalResponse);

            }
            catch (Exception ex)
            {
                log.LogInformation("Exception : " + ex.Message);
            }
            return new OkObjectResult(finalResponse);


        }
    }
}
